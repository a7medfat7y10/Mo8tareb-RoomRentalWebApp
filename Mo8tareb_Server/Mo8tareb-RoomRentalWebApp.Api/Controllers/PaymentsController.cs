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
using Mo8tareb_RoomRentalWebApp.BL.Managers.PaymentManagers;
using FluentValidation;

namespace Mo8tareb_RoomRentalWebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentManager _paymentManager;
        private readonly IConfiguration _configuration;
        private readonly IValidator<CreateCheckoutSessionRequest> _createCheckoutSessionValidator;

        public PaymentsController(IConfiguration configuration, UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IPaymentManager paymentManager, IValidator<CreateCheckoutSessionRequest> createCheckoutSessionValidator)
        {
            _configuration = configuration;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _paymentManager = paymentManager;
            _createCheckoutSessionValidator = createCheckoutSessionValidator;
        }

        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CreateCheckoutSessionRequest req)
        {
            var validationResult = _createCheckoutSessionValidator.Validate(req);
            
            if (!validationResult.IsValid)
                return BadRequest(new { StatusCode = 400, Errors = validationResult.Errors.ToDictionary(i => i.PropertyName, i => i.ErrorMessage) });

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
                                Name = req.RoomId.ToString(),
                                Description =  req.RoomDescription,
                                //Images = req.RoomImages
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

        [HttpPost]
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
                        reservation.Status = ReservationStatus.Approved;
                        _unitOfWork.Reservations.Update(reservation);
                        await _unitOfWork.SaveAsync();
                    }
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

        [HttpGet] // TODO: Add authrorization , or else anyone can see all payments
        public async Task<IActionResult> GetAllPayments()
        {
            var result = await _paymentManager.GetAllPayments();
            if (result != null)
                return Ok(result);

            return BadRequest("No payments available");
        }

        [HttpGet("{paymentId}")] // TODO: Add authrorization
        public async Task<IActionResult> GetPaymentById([FromRoute] int paymentId)
        {
            var result = await _paymentManager.GetPaymentById(paymentId);
            if (result != null)
                return Ok(result);

            return BadRequest("Invalid payment Id");
        }

    }
}
