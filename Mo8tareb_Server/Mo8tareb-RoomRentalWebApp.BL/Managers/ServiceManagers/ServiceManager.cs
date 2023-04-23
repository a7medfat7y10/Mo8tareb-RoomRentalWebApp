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

        public async Task<ServicesUpdateDtos?>? UpdateService(ServicesUpdateDtos service)
        {
            Service? serviceFromDatabase = _UnitOfWork.Services.FindByCondtion(r => r.Id == service.id).FirstOrDefault(); 
            if (serviceFromDatabase == null)
                return null;

            serviceFromDatabase.Name = service.Name;

            try
            {
                _UnitOfWork.Services.Update(serviceFromDatabase);
                int rowsAffected = await _UnitOfWork.SaveAsync();
                if (rowsAffected <= 0) throw new Exception();
            }
            catch
            {
                return null;
            }
            return service;

        }
        public async Task<ServicesToDeleteDtos?>? DeleteService(ServicesToDeleteDtos service)
        {
            Service? serviceFromDatabase = _UnitOfWork.Services.FindByCondtion(r => r.Id == service.id).FirstOrDefault();
            if (serviceFromDatabase == null)
                return null;

            try
            {
                _UnitOfWork.Services.Remove(serviceFromDatabase);
                int rowsAffected = await _UnitOfWork.SaveAsync();
                if (rowsAffected <= 0) throw new Exception();
            }
            catch { return null; }

            return service;
        }

    }
}
