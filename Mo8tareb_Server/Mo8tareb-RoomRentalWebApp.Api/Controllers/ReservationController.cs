using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mo8tareb_RoomRentalWebApp.Api.JwtFeatures;
using Mo8tareb_RoomRentalWebApp.Api.Payloads;
using Mo8tareb_RoomRentalWebApp.BL.Managers.ReservationManager;

namespace Mo8tareb_RoomRentalWebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationManager _reservationManager;
        private readonly JwtHandler _jwtHandler;

        public ReservationController(IReservationManager reservationManager, JwtHandler jwtHandler)
        {
            _reservationManager = reservationManager;
            _jwtHandler = jwtHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationPayload payload)
        {
            // TODO Login and Get token and get user id from token and pass it to function
            var result = await _reservationManager.CreateReservationAsync(payload, "123456");
            return Ok(result);
        }
    }
}
