using Microsoft.AspNetCore.Identity;
using Mo8tareb_RoomRentalWebApp.BL.Dtos._ٌReviewsDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.OwnerDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.ServieDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.UserDtos;
using Mo8tareb_RoomRentalWebApp.DAL;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace Mo8tareb_RoomRentalWebApp.BL.Managers.ServiceManagers
{
    public class ServiceManager : IServiceManager
    {
        public ServiceManager(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public IUnitOfWork _UnitOfWork { get; }

        public async Task<IQueryable<ServiceReadDtos>> GetAllServicesWithRoomsAsync()
        {
            var services = await _UnitOfWork.Services.GetAllServicesWithRooms();

            var ServicesDtos = services.Select(s =>
            new ServiceReadDtos()
            {
                Id = s.Id,
                Name = s.Name,
                Rooms = s.Rooms.Select(r => new RoomChildDto()
                {
                    Id = r.Id,
                    Location = r.Location,
                    RoomType = r.RoomType,
                    Price = r.Price
                }).ToList(),
            });
            return ServicesDtos;
        }

        public async Task<ServiceReadDtos> CreateService(ServiceReadDtos createServiceDto)
        {
                Service CreatedService = new Service()
                {
                    Id = createServiceDto.Id,
                    Name = createServiceDto.Name,
                };
                await _UnitOfWork.Services.AddAsync(CreatedService);
                int rowsAffected = await _UnitOfWork.SaveAsync();

                return rowsAffected > 0 ?createServiceDto: null;
        }

        public Task<ServiceReadDtos> DeleteService(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServicesToDeleteDtos?>? DeleteServiceAsync(ServicesToDeleteDtos service)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<ServiceReadDtos>> GetAllServices()
        {
            throw new NotImplementedException();
        }


        public Task<ServiceReadDtos?> GetDetailsById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceReadDtos> GetServiceById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceReadDtos> UpdateService(int id, ServiceReadDtos updateServiceDto)
        {
            throw new NotImplementedException();
        }

        public Task<ServicesUpdateDtos?>? UpdateServiceAsync(ServicesUpdateDtos service)
        {
            throw new NotImplementedException();
        }
    }
}
