using Microsoft.EntityFrameworkCore;
using Mo8tareb_RoomRentalWebApp.DAL;
using Mo8tareb_RoomRentalWebApp.DAL.Models;

namespace Mo8tareb_RoomRentalWebApp.BL.Managers.PaymentManagers
{
    public class PaymentManager : IPaymentManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Payment>> GetAllPayments()
        {
            var payments = await (await _unitOfWork.Payments.GetAllAsync()).ToListAsync();
            return payments;
        }

        public async Task<Payment> GetPaymentById(int paymentId)
        {
            var payment = await _unitOfWork.Payments.FindByCondtion(i => i.Id == paymentId).FirstOrDefaultAsync();
            return payment;
        }
    }
}
