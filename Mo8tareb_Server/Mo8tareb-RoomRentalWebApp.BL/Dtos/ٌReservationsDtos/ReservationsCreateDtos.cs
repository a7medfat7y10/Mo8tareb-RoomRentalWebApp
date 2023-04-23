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
    public record ReservationsCreateDtos(DateTime StartDate, DateTime EndDate, ReservationStatus Status, userCreateDtos User, RoomCreateDtos Room);
    public record ReservationsUpdateDtos(int id, DateTime StartDate, DateTime EndDate, ReservationStatus Status);
    public record ReservationsToDeleteDtos(int id);

}
