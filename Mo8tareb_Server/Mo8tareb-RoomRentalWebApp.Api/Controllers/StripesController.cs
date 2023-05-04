using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using Stripe;
using System.Security.Claims;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.PaymentDtos;
using Mo8tareb_RoomRentalWebApp.DAL;
using Org.BouncyCastle.Ocsp;
using Mo8tareb_RoomRentalWebApp.Api.Services.Email;
using System.Text.Encodings.Web;

namespace Mo8tareb_RoomRentalWebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripesController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        public StripesController(IConfiguration configuration,
            IEmailSender emailSender,
            UserManager<AppUser> userManager,
            IUnitOfWork unitOfWork
            )
        {
            _configuration = configuration;
            _emailSender = emailSender;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CreateCheckoutSessionRequest req)
        {
            SessionCreateOptions? options = new SessionCreateOptions
            {
                SuccessUrl = req.SuccessUrl,
                CancelUrl = req.FailureUrl,
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                Mode = "payment",
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "USD",
                            UnitAmount = req.RoomPrice,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = req.RoomTitle,
                                Description =  req.RoomDescription,
                                Images = req.RoomImages
                            },
                        },

                    },
                },
                Metadata = new Dictionary<string, string>
                {
                  { "reservation_id", req.ReservationId.ToString() }
                },
            };

            SessionService? service = new SessionService();

            try
            {
                Session? session = await service.CreateAsync(options);

                return Ok(new CreateCheckoutSessionResponse
                {
                    SessionId = session!.Id,
                    PublicKey = _configuration["Stripe:PublicKey"]!
                });
            }
            catch (StripeException e)
            {
                Console.WriteLine(e.StripeError.Message);
                return BadRequest(new ErrorResponse
                {
                    ErrorMessage = new ErrorMessage
                    {
                        Message = e.StripeError.Message,
                    }
                });
            }
        }


        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            string? json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            string endpointSecret = _configuration["Stripe:WebHook_Sec"]!;
            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);
                var signatureHeader = Request.Headers["Stripe-Signature"];

                stripeEvent = EventUtility.ConstructEvent(json,
                        signatureHeader, endpointSecret);

                await Console.Out.WriteLineAsync(stripeEvent.Type);

                if (stripeEvent.Type == "checkout.session.completed")
                {
                    Session? session = stripeEvent.Data.Object as Session;
                    int reservationId = int.Parse(session!.Metadata["reservation_id"]);

                    Reservation? reservation = await _unitOfWork.Reservations.GetByIdAsync((int)reservationId);

                    if (reservation != null)
                    {
                        reservation.Status = ReservationStatus.Pending;
                        _unitOfWork.Reservations.Update(reservation);
                        await _unitOfWork.SaveAsync();

                        // send notification to the owner to approve or reject the reservation payment

                        // Owner owner = await _customUserManager.FindOwnerByIdAsync(reservation?.Room?.OwnerId?.ToString()!);
                        Owner? owner = await _userManager.FindByIdAsync(reservation?.Room?.OwnerId?.ToString()!) as Owner;
                        AppUser? user = await _userManager.FindByIdAsync(reservation?.UserId?.ToString()!);

                        var callbackUrlApprove = $"{_configuration["AppUrl"]}/ApproveReservationPayment?reservationId={reservationId}";
                        var callbackUrlReject = $"{_configuration["AppUrl"]}/RejectReservationPayment?reservationId={reservationId}";

                        var message = new Message(new string[] { owner.Email! },
                                                  subject: "New Reservation Payment Request",
                                                  $"Hi {owner.UserName},<br/>User {user?.UserName} has made a new reservation payment request. " +
                                                  $"Please <a href='{HtmlEncoder.Default.Encode(callbackUrlApprove)}'>click here</a> to approve the request or " +
                                                  $"<a href='{HtmlEncoder.Default.Encode(callbackUrlReject)}'>click here</a> to reject the request."
                                                  , attachments: null!
                                                  , isHtml: false
                                                  );


                        await _emailSender.SendEmailAsync(message);
                    }

                    Payment payment = new Payment
                    {
                        StripeId = session.PaymentIntentId,
                        
                        ReservationId = reservation?.Id,
                        Amount = session.AmountTotal
                    };
                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine(e.StripeError.Message);
                return BadRequest();
            }
        }

        [HttpPost("ApproveReservationPayment")]
        public async Task<IActionResult> ApproveReservationPayment([FromQuery] int reservationId)
        {
            Reservation reservation = await _unitOfWork.Reservations.GetByIdAsync(reservationId);
            if (reservation != null && reservation.Status == ReservationStatus.Pending)
            {
                reservation.Status = ReservationStatus.Approved;
                _unitOfWork.Reservations.Update(reservation);
                await _unitOfWork.SaveAsync();

                // Get the user who made the reservation
                AppUser? user = await _userManager.FindByIdAsync(reservation.UserId!);

                // Send email to the user
                var message = new Message(new string[] { user.Email! },
                                           subject: "Mo8tareb Room Rental Web App: Reservation Approved",
                                           content: GetApprovalEmailHtml(user, reservation),
                                           attachments: null!,
                                           isHtml: true
                                          );
                await _emailSender.SendEmailAsync(message);

                return Ok();
            }
            return NotFound();
        }

        [HttpPost("RejectReservationPayment")]
        public async Task<IActionResult> RejectReservationPayment([FromQuery] int reservationId)
        {
            Reservation reservation = await _unitOfWork.Reservations.GetByIdAsync(reservationId);
            if (reservation != null && reservation.Status == ReservationStatus.Pending)
            {
                // Refund the payment
                Payment payment = await _unitOfWork.Payments.GetByReservationIdAsync(reservationId);
                if (payment != null)
                {
                    var refundOptions = new RefundCreateOptions
                    {
                        PaymentIntent = payment.StripeId,
                        Reason = RefundReasons.Fraudulent
                    };
                    var refundService = new RefundService();
                    await refundService.CreateAsync(refundOptions);
                }

                // Update the reservation status
                reservation.Status = ReservationStatus.Rejected;
                _unitOfWork.Reservations.Update(reservation);
                await _unitOfWork.SaveAsync();

                // Send notification to the user
                var user = await _userManager.FindByIdAsync(reservation?.UserId?.ToString()!);

                var message = new Message(new string[] { user.Email! },
                                                  subject: "Reservation Payment Rejected",
                                                  $"Hi {user.UserName},<br/>Your reservation payment has been rejected." +
                                                  $" Please contact the room owner for more information.",
                                                  attachments: null!,
                                                  isHtml: false
                                                 );

                await _emailSender.SendEmailAsync(message);

                return Ok();
            }
            return NotFound();
        }


        [Authorize]
        [HttpPost("customer-portal")]
        public async Task<IActionResult> CustomerPortal([FromBody] CustomerPortalRequest req)
        {

            try
            {
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                Claim? claim = principal.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname");
                AppUser? userFromDb = await _userManager.FindByNameAsync(claim.Value);

                if (userFromDb == null)
                {
                    return BadRequest();
                }

                var options = new Stripe.BillingPortal.SessionCreateOptions
                {
                    Customer = userFromDb.Id,
                    ReturnUrl = req.ReturnUrl,
                };

                var service = new Stripe.BillingPortal.SessionService();
                var session = await service.CreateAsync(options);

                return Ok(new
                {
                    url = session.Url
                });
            }
            catch (StripeException e)
            {
                Console.WriteLine(e.StripeError.Message);
                return BadRequest(new ErrorResponse
                {
                    ErrorMessage = new ErrorMessage
                    {
                        Message = e.StripeError.Message,
                    }
                });
            }

        }

        private string GetApprovalEmailHtml(AppUser user, Reservation reservation)
        {
            return $@"<html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    font-size: 16px;
                    color: #333;
                    text-align: center;
                }}
            </style>
        </head>
        <body>
            <h2>Your reservation request has been approved!</h2>
            <p>Dear {user.FirstName},</p>
            <p>We are pleased to inform you that your reservation request for the room with ID {reservation.RoomId} has been approved.</p>
            <p>Please note that the payment of {reservation?.Payment?.Amount} has been deducted from your account.</p>
            <p>If you have any questions, please do not hesitate to contact us on this Phone {reservation?.Room?.Owner?.PhoneNumber}.</p>
            <p>Best regards,</p>
            <p>Mo8tareb Room Rental Web App Team</p>
            <h4>P.S. - You can check out our website to find more information about our services and offers: <a href=""[Mo8tareb Web App URL]"">Mo8tareb Web App URL</a></h4>
        </body>
    </html>";
        }



    }
}
