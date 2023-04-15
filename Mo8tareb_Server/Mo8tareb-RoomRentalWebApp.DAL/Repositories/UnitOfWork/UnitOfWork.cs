using Mo8tareb_RoomRentalWebApp.DAL.Context;
using Mo8tareb_RoomRentalWebApp.DAL.Repositories.ReviewRepo;
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

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();

    }
}
