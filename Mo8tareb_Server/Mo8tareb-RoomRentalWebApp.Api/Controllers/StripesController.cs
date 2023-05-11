using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Stripe;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.PaymentDtos;
using Mo8tareb_RoomRentalWebApp.DAL;
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
            StripeConfiguration.ApiKey = _configuration["Stripe:Secret_key"];

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
                            UnitAmount = req.RoomPrice * 100,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = req.RoomDescription,
                                Description =  req.RoomDescription,
                               //Images = req.RoomImages
                            },
                        },
                        Quantity = 1,

                    },
                },
                //Metadata = new Dictionary<string, string>
                //{
                //  { "reservation_id", req.ReservationId.ToString() }
                //},
            };
            SessionService? service = new SessionService();
           // service.Create(options);
      

            try
            {
                Session? session = await service.CreateAsync(options);

                Payment payment = new Payment
                {
                    StripeId = session.PaymentIntentId,
                    ReservationId = req.ReservationId,
                    Amount = session.AmountTotal
                };

                // update room_reserved_status
                Room? room = await _unitOfWork.Rooms.GetRoom(req.RoomId);

                room.IsReserved = true;
                _unitOfWork.Rooms.Update(room);
                await _unitOfWork.SaveAsync();

                return Ok(new CreateCheckoutSessionResponse
                {
                    SessionId = session!.Id,
                    PublicKey = _configuration["Stripe:Publishable_key"]!
                });
            }
            catch (StripeException e)
            {
                Console.WriteLine(e?.StripeError.Message);
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
    }
}
