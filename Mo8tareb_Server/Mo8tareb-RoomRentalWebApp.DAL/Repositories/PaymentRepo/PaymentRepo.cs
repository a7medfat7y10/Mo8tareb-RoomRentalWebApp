using Mo8tareb_RoomRentalWebApp.DAL.Context;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.DAL.Repositories.PaymentRepo
{
    public class PaymentRepo : GenericRepo<Payment>, IPaymentRepo
    {
        public PaymentRepo(ApplicationDbContext context) : base(context)
        {
        }
    }
}
