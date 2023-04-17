using Mo8tareb_RoomRentalWebApp.DAL.Context;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.DAL.Repositories.ReservationRepo
{
    public class ReservationRepo : GenericRepo<Reservation>, IReservationRepo
    {
        public ReservationRepo(ApplicationDbContext context) : base(context)
        {
        }
    }
}
