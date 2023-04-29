using Microsoft.AspNetCore.Mvc;
using Mo8tareb_RoomRentalWebApp.BL.Dtos._ٌReviewsDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.ReservationsDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos;
using Mo8tareb_RoomRentalWebApp.BL.Managers.RoomManagers;
using Mo8tareb_RoomRentalWebApp.BL.Managers.ServiceManagers;
using Mo8tareb_RoomRentalWebApp.DAL.Models;

namespace Mo8tareb_RoomRentalWebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : Controller
    {
        public RoomsController(IRoomManager roomManager)
        {
            RoomManager = roomManager;
        }

        public IRoomManager RoomManager { get; }

        [HttpGet]
        [Route("GetAllRooms")]
        public async Task<IActionResult> GetAllRooms()
        {
            var lst = await RoomManager.GetAllRoomsWithDetails();

            return lst.Count() == 0 ? NotFound() : Ok(lst);
        }

        [HttpGet]
        [Route("GetRoomById")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var room = await RoomManager.GetRoomWithDetails(id);

            return room == null ? NotFound() : Ok(room);
        }

        [HttpPost]
        [Route("CreateRoom")]
        public async Task<object> CreateRoom(RoomCreateDto room)
        {
            if (room == null || !ModelState.IsValid)
                return BadRequest("Please send a Valid data to create !!");

            Room? objectCreated = await RoomManager?.CreateRoomsAsync(room)!;

            return objectCreated != null ? objectCreated.Id : BadRequest("Could not create Room due to the inValid data you sent :(");
        }

        [HttpPut]
        [Route("UpdateRoom")]
        public async Task<IActionResult> UpdateRoom(int id, RoomUpdateDto room)
        {
            if (room == null || id != room.id)
                return BadRequest("Please send valid Data to Update !!");

            RoomUpdateDto? objectUpdated = await RoomManager.UpdateRoomAsync(room)!;

            return objectUpdated != null ? Ok("Room Updated Succssfuly !") : BadRequest("Could not Update Room due to the inValid data you sent  :(");
        }

        [HttpDelete]
        [Route("DeleteRoom")]
        public async Task<IActionResult> DeleteRoom(int id, RoomDeleteDto room)
        {
            if (room == null || id != room.id)
                return BadRequest("Please send valid Data to Update !!");

            RoomDeleteDto? objectUpdated = await RoomManager.DeleteRoomAsync(room)!;

            return objectUpdated != null ? Ok("Room Deleted Succssfuly !") : BadRequest("Could not Deleted Room due to the inValid data you sent  :(");
        }
    }
}
