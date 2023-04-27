using Stripe;

namespace Mo8tareb_RoomRentalWebApp.Api.services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;

        public PaymentService(IConfiguration configuration)
        {
            _configuration = configuration;
            StripeConfiguration.ApiKey = _configuration.GetSection("Stripe")["SecretKey"];
        }

        public async Task<PaymentResult> ProcessPaymentAsync(PaymentDto paymentDto)
        {
            ChargeCreateOptions? options = new ChargeCreateOptions
            {
                Amount = (long)(paymentDto.Amount * 100),
                Currency = "usd",
                Description = $"Payment for reservation {paymentDto.ReservationId}",
                Source = paymentDto.StripeToken
            };

            ChargeService? service = new ChargeService();
            try
            {
                Charge? charge = await service.CreateAsync(options);
                return new PaymentResult { Success = true, ChargeId = charge.Id };
            }
            catch (StripeException ex)
            {
                return new PaymentResult { Success = false, ErrorMessage = ex.Message };
            }
        }
    }

}

/*
 * public class PaymentService : IPaymentService
{
    private readonly StripeConfiguration _stripeConfiguration;

    public PaymentService(IOptions<StripeConfiguration> stripeConfiguration)
    {
        _stripeConfiguration = stripeConfiguration.Value;
    }

    public async Task<bool> CreatePaymentAsync(PaymentDto paymentDto)
    {
        try
        {
            StripeConfiguration.ApiKey = _stripeConfiguration.SecretKey;

            var options = new ChargeCreateOptions
            {
                Amount = (int)(paymentDto.Amount * 100),
                Currency = paymentDto.Currency,
                Source = new StripeSourceOptions
                {
                    Token = new StripeTokenCreateOptions
                    {
                        Card = new StripeCreditCardOptions
                        {
                            Number = paymentDto.CardNumber,
                            ExpMonth = paymentDto.ExpiryMonth,
                            ExpYear = paymentDto.ExpiryYear,
                            Cvc = paymentDto.Cvc
                        }
                    }
                },
                Description = $"Payment for reservation {paymentDto.ReservationId}"
            };

            var service = new ChargeService();
            var charge = await service.CreateAsync(options);

            if (charge.Status == "succeeded")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (StripeException ex)
        {
            // Handle Stripe errors
            return false;
        }
        catch (Exception ex)
        {
            // Handle other errors
            return false;
        }
    }
}

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment(PaymentDto paymentDto)
    {
        var result = await _paymentService.CreatePaymentAsync(paymentDto);

        if (result)
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }
}
*/
