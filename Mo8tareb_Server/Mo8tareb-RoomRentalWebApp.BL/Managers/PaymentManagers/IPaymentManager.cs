using Mo8tareb_RoomRentalWebApp.DAL.Models;

namespace Mo8tareb_RoomRentalWebApp.BL.Managers.PaymentManagers
{
    public interface IPaymentManager
    {
        Task<List<Payment>> GetAllPayments();
        Task<Payment> GetPaymentById(int paymentId);
    }
}
