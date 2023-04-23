using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.ReservationsDtos;
using Mo8tareb_RoomRentalWebApp.BL.Managers.ReservationManagers;
using Mo8tareb_RoomRentalWebApp.BL.Managers.ReviewManagers;
using Mo8tareb_RoomRentalWebApp.DAL.Context;

namespace Mo8tareb_RoomRentalWebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        public readonly IReservationManager _ReservationManager;

        public ReservationsController(IReservationManager ReservationManager) => _ReservationManager = ReservationManager;


        [HttpGet]
        [Route("GetAllReservationsWithUsersWithRoomsAsync")]
        public async Task<IActionResult> GetAllReservationsWithUsersWithRoomsAsync()
        {
            IQueryable<ReservationsReadDtos>? ReservationsReadDtos = await _ReservationManager.GetAllReservationsWithUsersWithRoomsAsync();

            return ReservationsReadDtos.Any() ? NotFound() : Ok(ReservationsReadDtos);
        }

        [HttpPost]
        [Route("CreateReservation")]
        public async Task<IActionResult> CreateReservation(ReservationsCreateDtos Reservation)
        {
            if (Reservation is null || !ModelState.IsValid)
                return BadRequest("Please send a Valid data to create !!");

            ReservationsCreateDtos? objectCreated = await _ReservationManager?.CreateReservationWithUsersWithRoomsAsync(Reservation)!;

            return objectCreated is not null ? Ok("Reservation created Succssfuly !") : BadRequest("Could not create Reservation due to the inValid data you sent :(");
        }


        [HttpPut]
        [Route("UpdateReservation")]
        public async Task<IActionResult> UpdateReservation(int id, ReservationsUpdateDtos Reservation)
        {
            if (Reservation is null || id != Reservation.id)
                return BadRequest("Please send valid Data to Update !!");

            ReservationsUpdateDtos? objectUpdated = await _ReservationManager.UpdateReservationAsync(Reservation)!;

            return objectUpdated is not null ? Ok("Reservation Updated Succssfuly !") : BadRequest("Could not Update Reservation due to the inValid data you sent  :(");
        }

        [HttpDelete]
        [Route("DeleteReservation")]
        public async Task<IActionResult> DeleteReservation(int id, ReservationsToDeleteDtos Reservation)
        {
            if (Reservation is null || id != Reservation.id)
                return BadRequest("Please send valid Data to Update !!");

            ReservationsToDeleteDtos? objectUpdated = await _ReservationManager.DeleteReservationAsync(Reservation)!;

            return objectUpdated is not null ? Ok("Reservation Deleted Succssfuly !") : BadRequest("Could not Deleted Reservation due to the inValid data you sent  :(");
        }


    }
}
