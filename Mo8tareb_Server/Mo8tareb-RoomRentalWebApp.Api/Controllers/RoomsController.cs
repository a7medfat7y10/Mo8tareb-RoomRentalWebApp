using Microsoft.AspNetCore.Mvc;
using Mo8tareb_RoomRentalWebApp.BL.Managers.RoomManagers;
using Mo8tareb_RoomRentalWebApp.BL.Managers.ServiceManagers;

namespace Mo8tareb_RoomRentalWebApp.Api.Controllers
{
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

        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
