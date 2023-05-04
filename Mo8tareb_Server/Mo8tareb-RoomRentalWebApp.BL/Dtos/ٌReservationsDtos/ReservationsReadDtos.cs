using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.UserDtos;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.ReservationsDtos
{
    public record ReservationsReadDtos(int id, DateTime StartDate, DateTime EndDate, ReservationStatus Status, userReadDtos User,RoomReadDtos Room);

    public class ReservationsReadDtosV2
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ReservationStatus Status { get; set; }
        public userReadDtosV2? User { get; set; }
        public RoomReadDtosV2? Room { get; set; }

    }

}
