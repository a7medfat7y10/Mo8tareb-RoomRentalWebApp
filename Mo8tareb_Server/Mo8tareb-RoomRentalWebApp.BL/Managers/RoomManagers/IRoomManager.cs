using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.ServieDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Managers.RoomManagers
{
    public interface IRoomManager
    {
        Task<IQueryable<RoomReadDto>> GetAllRoomsWithDetails();

        //Task<ServiceReadDtos> CreateService(ServiceReadDtos createServiceDto);

        //Task<IQueryable<ServiceReadDtos>> GetAllServices();
        //Task<ServiceReadDtos> GetServiceById(int id);
        //Task<ServiceReadDtos?> GetDetailsById(int id);
        //Task<ServiceReadDtos> UpdateService(int id, ServiceReadDtos updateServiceDto);
        //Task<ServiceReadDtos> DeleteService(int id);
        //Task<ServicesUpdateDtos?>? UpdateServiceAsync(ServicesUpdateDtos service);
        //Task<ServicesToDeleteDtos?>? DeleteServiceAsync(ServicesToDeleteDtos service);
    }
}
