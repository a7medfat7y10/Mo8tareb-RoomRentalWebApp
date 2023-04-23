using Microsoft.EntityFrameworkCore;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.DAL.Repositories.ReservationRepo
{
    public interface IReservationRepo : IGenericRepo<Reservation>
    {
        public  Task<IQueryable<Reservation>> GetAllReservationsWithUsersWithRoomsRepoFuncAsync();

    }
}
