namespace Mo8tareb_RoomRentalWebApp.Api.services.Payment
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessPaymentAsync(PaymentDto paymentDto);
    }
}
