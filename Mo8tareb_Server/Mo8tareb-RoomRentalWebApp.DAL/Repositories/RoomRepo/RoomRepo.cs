using Microsoft.EntityFrameworkCore;
using Mo8tareb_RoomRentalWebApp.DAL.Context;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.DAL.Repositories.RoomRepo
{
    public class RoomRepo : GenericRepo<Room>, IRoomRepo
    {
        public RoomRepo(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IQueryable<Room>> GetAllRoomsWithAllDetails()
        {
            return await Task.FromResult(_context.Rooms.Include(r => r.Reservations)
                                                       .Include(r => r.Owner)
                                                       .Include(r => r.Services)
                                                       .Include(r => r.Images)
                                                       .Include(r => r.Reviews));
        }
        public async Task<Room> GetRoom(int id)
        {
            return await Task.FromResult(_context.Rooms.Include(r => r.Reservations)
                                                       .Include(r => r.Owner)
                                                       .Include(r => r.Services)
                                                       .Include(r => r.Images)
                                                       .Include(r => r.Reviews).FirstOrDefault(r=> r.Id == id));
        }
    }
}
