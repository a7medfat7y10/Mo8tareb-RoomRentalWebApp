namespace Mo8tareb_RoomRentalWebApp.Api.services.Payment
{
    public class PaymentDto
    {
        public int ReservationId { get; set; }
        public decimal Amount { get; set; }
        public string StripeToken { get; set; }
    }

}
