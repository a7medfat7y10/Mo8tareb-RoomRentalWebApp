using Microsoft.EntityFrameworkCore;
using Mo8tareb_RoomRentalWebApp.DAL.Context;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using Mo8tareb_RoomRentalWebApp.DAL.Repositories.ReviewRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.DAL.Repositories.ServiceRepo
{
    public class ServiceRepo : GenericRepo<Service>, IServiceRepo
    {
        public ServiceRepo(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IQueryable<Service>> GetAllServicesWithRooms()
        {
             return await Task.FromResult(_context.Services.Include(r => r.Rooms));
        }
    }
}
