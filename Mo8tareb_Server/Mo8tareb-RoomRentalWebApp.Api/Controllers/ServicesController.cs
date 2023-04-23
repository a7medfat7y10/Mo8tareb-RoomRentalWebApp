using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mo8tareb_RoomRentalWebApp.BL.Dtos._ٌReviewsDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.ServieDtos;
using Mo8tareb_RoomRentalWebApp.BL.Managers.RoomManagers;
using Mo8tareb_RoomRentalWebApp.BL.Managers.ServiceManagers;
using Mo8tareb_RoomRentalWebApp.DAL.Models;

namespace Mo8tareb_RoomRentalWebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        public ServicesController(IServiceManager serviceManager) 
        {
            ServiceManager = serviceManager;
        }

        public IServiceManager ServiceManager { get; }

        [HttpGet]
        [Route("GetAllServies")]
        public async Task<IActionResult> GetAllServies()
        {
            var lst = await ServiceManager.GetAllServicesWithRoomsAsync();

            return lst.Count() == 0 ? NotFound() : Ok(lst);
        }

        [HttpPost]
        [Route("CreateService")]
        public async Task<IActionResult> CreateService(ServiceReadDtos service)
        {
            if (service == null || !ModelState.IsValid)
                return BadRequest("Please send a Valid data to create !!");

            ServiceReadDtos? objectCreated = await ServiceManager?.CreateService(service);

            return objectCreated != null ? Ok("Service created Succssfuly !") : BadRequest("Could not create Service due to the inValid data you sent :(");
        }

        [HttpPut]
        [Route("UpdateService")]
        public async Task<IActionResult> UpdateService(int id, ServicesUpdateDtos service)
        {
            if (service == null || id != service.id)
                return BadRequest("Please send valid Data to Update !!");

            ServicesUpdateDtos? objectUpdated = await ServiceManager.UpdateService(service)!;

            return objectUpdated != null ? Ok("Service Updated Succssfuly !") : BadRequest("Could not Update Service due to the inValid data you sent  :(");
        }

        [HttpDelete]
        [Route("DeleteService")]
        public async Task<IActionResult> DeleteService(int id, ServicesToDeleteDtos service)
        {
            if (service == null || id != service.id)
                return BadRequest("Please send valid Data to Update !!");

            ServicesToDeleteDtos? objectUpdated = await ServiceManager.DeleteService(service)!;

            return objectUpdated != null ? Ok("Service Deleted Succssfuly !") : BadRequest("Could not Deleted Service due to the inValid data you sent  :(");
        }


    }
}
