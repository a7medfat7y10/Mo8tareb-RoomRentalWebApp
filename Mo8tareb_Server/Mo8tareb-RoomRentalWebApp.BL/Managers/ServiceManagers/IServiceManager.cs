using Mo8tareb_RoomRentalWebApp.BL.Dtos._ٌReviewsDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.ServieDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Managers.ServiceManagers
{
    public interface IServiceManager
    {
        Task<ServiceReadDtos> CreateService(ServiceReadDtos createServiceDto);
        Task<IQueryable<ServiceReadDtos>> GetAllServicesWithRoomsAsync();
        public Task<ServicesUpdateDtos?>? UpdateService(ServicesUpdateDtos service);
        public Task<ServicesToDeleteDtos?>? DeleteService(ServicesToDeleteDtos service);

    }
}
