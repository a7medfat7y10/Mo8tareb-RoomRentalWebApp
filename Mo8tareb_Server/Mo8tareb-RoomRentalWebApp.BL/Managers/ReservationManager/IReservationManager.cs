using Mo8tareb_RoomRentalWebApp.Api.Payloads;
using Mo8tareb_RoomRentalWebApp.DAL.Models;

namespace Mo8tareb_RoomRentalWebApp.BL.Managers.ReservationManager
{
    public interface IReservationManager
    {
        Task<Reservation> CreateReservationAsync(ReservationPayload payload, string userId);
    }
}
