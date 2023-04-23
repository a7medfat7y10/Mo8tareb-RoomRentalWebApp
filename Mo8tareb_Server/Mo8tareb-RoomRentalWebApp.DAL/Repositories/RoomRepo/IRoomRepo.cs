using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.DAL.Repositories.RoomRepo
{
    public interface IRoomRepo: IGenericRepo<Room>
    {
        public Task<IQueryable<Room>> GetAllRoomsWithAllDetails();
        public Task<Room> GetRoom(int id);

    }
}
