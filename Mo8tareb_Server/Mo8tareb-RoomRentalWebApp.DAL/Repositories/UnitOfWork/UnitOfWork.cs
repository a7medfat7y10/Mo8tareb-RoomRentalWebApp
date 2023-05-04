using Mo8tareb_RoomRentalWebApp.DAL.Context;
using Mo8tareb_RoomRentalWebApp.DAL.Repositories.PaymentRepo;
using Mo8tareb_RoomRentalWebApp.DAL.Repositories.ReservationRepo;
using Mo8tareb_RoomRentalWebApp.DAL.Repositories.ReviewRepo;
using Mo8tareb_RoomRentalWebApp.DAL.Repositories.RoomRepo;
using Mo8tareb_RoomRentalWebApp.DAL.Repositories.ServiceRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Mo8tareb_RoomRentalWebApp.DAL
{
    public class UnitOfWork : IUnitOfWork
    {

        private ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context) => _context = context;
        
        public IReviewRepo Reviews => new ReviewRepo(_context);
        //public IOwnerRepo Owners => new OwnerRepo(_context);
        //public IUserRepo Users => new UserRepo(_context);
        public IPaymentRepo Payments => new PaymentRepo(_context);
        public IReservationRepo Reservations => new ReservationRepo(_context);
        public IServiceRepo Services => new ServiceRepo(_context);
        public IRoomRepo Rooms => new RoomRepo(_context);


        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();

    }
}
