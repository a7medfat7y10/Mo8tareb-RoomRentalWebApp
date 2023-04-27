namespace Mo8tareb_RoomRentalWebApp.Api.services.Payment
{
    public class PaymentResult
    {
        public bool Success { get; set; }
        public string ChargeId { get; set; }
        public string ErrorMessage { get; set; }
    }

}
