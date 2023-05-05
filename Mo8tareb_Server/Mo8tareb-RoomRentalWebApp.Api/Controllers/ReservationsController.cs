using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.ReservationsDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos;
using Mo8tareb_RoomRentalWebApp.BL.Managers.ReservationManagers;
using Mo8tareb_RoomRentalWebApp.BL.Managers.ReviewManagers;
using Mo8tareb_RoomRentalWebApp.BL.Managers.RoomManagers;
using Mo8tareb_RoomRentalWebApp.DAL.Context;
using Mo8tareb_RoomRentalWebApp.DAL.Models;

namespace Mo8tareb_RoomRentalWebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        public readonly IReservationManager _ReservationManager;
        public readonly IRoomManager _RoomManager;
        public readonly UserManager<AppUser> _userManger;

        public ReservationsController(UserManager<AppUser> userManager, IRoomManager RoomManager , IReservationManager ReservationManager)
        {
            _ReservationManager = ReservationManager;
            _RoomManager= RoomManager;
            _userManger = userManager;
        }


        [HttpPost]
        [Route("DidThisUserReserveThisRoom")]
        public async Task<IActionResult> DidThisUserReserveThisRoom([FromBody] EmailRoomIdDto obj)
        {
           var user= await _userManger.FindByEmailAsync(obj.userEmail);
           var room = await _RoomManager.GetRoomWithDetails(obj.RoomId);
           if (user == null || room == null) 
                return NotFound();

            return _ReservationManager.DidThisUserReserveThisRoomManager(user, room)==true?Ok(true):  Ok(false);
        }

        [HttpPost]
        [Route("DidThisUserReserveThisRoomAndGetApprovedByOwner")]
        public async Task<IActionResult> DidThisUserReserveThisRoomAndGetApprovedByOwner([FromBody] EmailRoomIdDto obj)
        {
            var user = await _userManger.FindByEmailAsync(obj.userEmail);
            var room = await _RoomManager.GetRoomWithDetails(obj.RoomId);
            if (user == null || room == null)
                return NotFound();

            return _ReservationManager.DidThisUserReserveThisRoomAndGetApprovedByOwnerManager(user, room) == true ? Ok(true) : Ok(false);
        }
        [HttpPost]
        [Route("DidThisUserReserveThisRoomAndGetRejectedByOwner")]
        public async Task<IActionResult> DidThisUserReserveThisRoomAndGetRejectedByOwner([FromBody] EmailRoomIdDto obj)
        {
            var user = await _userManger.FindByEmailAsync(obj.userEmail);
            var room = await _RoomManager.GetRoomWithDetails(obj.RoomId);
            if (user == null || room == null)
                return NotFound();

            return _ReservationManager.DidThisUserReserveThisRoomAndGetRejectedByOwnerManager(user, room) == true ? Ok(true) : Ok(false);
        }
        [HttpPost]
        [Route("DidThisUserReserveThisRoomAndGetSuspendedByOwner")]
        public async Task<IActionResult> DidThisUserReserveThisRoomAndGetSuspendedByOwner([FromBody] EmailRoomIdDto obj)
        {
            var user = await _userManger.FindByEmailAsync(obj.userEmail);
            var room = await _RoomManager.GetRoomWithDetails(obj.RoomId);
            if (user == null || room == null)
                return NotFound();

            return _ReservationManager.DidThisUserReserveThisRoomAndGetSuspendedByOwnerManager(user, room) == true ? Ok(true) : Ok(false);
        }

        [HttpGet]
        [Route("GetAllReservationsWithUsersWithRoomsAsync")]
        public async Task<IActionResult> GetAllReservationsWithUsersWithRoomsAsync()
        {
            IQueryable<ReservationsReadDtos>? ReservationsReadDtos = await _ReservationManager.GetAllReservationsWithUsersWithRoomsAsync();

            return ReservationsReadDtos.Count()==0 ? NotFound() : Ok(ReservationsReadDtos);
        }

        [HttpPost]
        [Route("CreateReservation")]
        public async Task<IActionResult> CreateReservation(ReservationsCreateDtos Reservation)
        {
            if (Reservation == null)
                return BadRequest("Please send a Valid data to create !!");

            ReservationsCreateDtos? objectCreated = await _ReservationManager?.CreateReservationWithUsersWithRoomsAsync(Reservation)!;

            Console.WriteLine("CreateReservation======================" + objectCreated);


            return objectCreated != null ? Ok("Reservation created Succssfuly !") : BadRequest("Could not create Reservation due to the inValid data you sent :(");
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

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetConfirmedUserReservationsByUserId([FromRoute] string userId)
        {
            var reservations = await _ReservationManager.GetConfirmedUserReservationsByUserId(userId);
            return Ok(reservations);
        }

        [HttpGet]
        [Route("by-email")]
        public async Task<IActionResult> GetConfirmedUserReservationsByEmail([FromQuery] string email)
        {
            if (email is null)
                return BadRequest("Email must be provided");
            var reservations = await _ReservationManager.GetConfirmedUserReservationsByUserEmail(email);
            return Ok(reservations);
        }

        [HttpPut]
        [Route("UpdateReservationStatus")]
        public async Task<IActionResult> UpdateReservationStatus(int id, ReservationsUpdateDtos Reservation)
        {
            if (Reservation is null || id != Reservation.id)
                return BadRequest("Please send valid Data to Update !!");

            ReservationsUpdateDtos? objectUpdated = await _ReservationManager.UpdateReservationStatus(Reservation)!;


            return objectUpdated is not null ? Ok("Reservation Updated Succssfuly !") : BadRequest("Could not Update Reservation due to the inValid data you sent  :(");
        }

    }
}
