using Mo8tareb_RoomRentalWebApp.BL.Dtos.ReservationsDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Managers.ReservationManagers
{
    public interface IReservationManager
    {
        Task<IQueryable<ReservationsReadDtos>> GetAllReservationsWithUsersWithRoomsAsync();
        Task<ReservationsCreateDtos?>? CreateReservationWithUsersWithRoomsAsync(ReservationsCreateDtos? createReservationDto);
        Task<ReservationsUpdateDtos?>? UpdateReservationAsync(ReservationsUpdateDtos Reservation);
        Task<ReservationsToDeleteDtos?>? DeleteReservationAsync(ReservationsToDeleteDtos Reservation);
    }
}
