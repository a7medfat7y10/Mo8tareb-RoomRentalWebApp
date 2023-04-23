using Microsoft.EntityFrameworkCore;
using Mo8tareb_RoomRentalWebApp.DAL.Context;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.DAL.Repositories.ReviewRepo
{
    public class ReviewRepo : GenericRepo<Review>, IReviewRepo
    {

        public ReviewRepo(ApplicationDbContext context) : base(context)
        {
            
        }


        public async Task<IQueryable<Review>> GetAllReviewsWithUsersWithRoomsRepoFuncAsync()
        {
             return await Task.FromResult(_context.Reviews.Include(r=>r.User).Include(r=>r.Room));
        }
    }
}
