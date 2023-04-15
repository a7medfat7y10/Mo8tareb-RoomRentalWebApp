using Microsoft.EntityFrameworkCore;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.DAL.Repositories.ReviewRepo
{
    public interface IReviewRepo:IGenericRepo<Review>
    {
        public  Task<IQueryable<Review>> GetAllReviewsWithUsersWithRoomsRepoFuncAsync();

    }
}
