using Mo8tareb_RoomRentalWebApp.DAL.Context;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using Mo8tareb_RoomRentalWebApp.DAL.Repositories.ReservationRepo;
using Mo8tareb_RoomRentalWebApp.DAL.Repositories.ReviewRepo;


namespace Mo8tareb_RoomRentalWebApp.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;
        public IReservationRepo ReservationRepo => new ReservationRepo(_context);
        public IReviewRepo Reviews => new ReviewRepo(_context);
        public UnitOfWork(ApplicationDbContext context) => _context = context;

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}
