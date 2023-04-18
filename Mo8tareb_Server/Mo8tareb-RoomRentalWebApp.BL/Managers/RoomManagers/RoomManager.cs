using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos.Childs;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.ServieDtos;
using Mo8tareb_RoomRentalWebApp.DAL;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Managers.RoomManagers
{
    public class RoomManager: IRoomManager
    {
        public RoomManager(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public IUnitOfWork _UnitOfWork { get; }

        public async Task<IQueryable<RoomReadDto>> GetAllRoomsWithDetails()
        {
            var rooms = await _UnitOfWork.Rooms.GetAllRoomsWithAllDetails();

            var RoomDtos = rooms.Select(r =>
            new RoomReadDto()
            {
                Id = r.Id,
                Price = r.Price,
                Location = r.Location,
                RoomType = r.RoomType,
                OwnerId = r.OwnerId,
                Services = r.Services.Select(s => new ServiceChildDto()
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList(),

                Reservations = r.Reservations.Select(r => new ReservationChildDto()
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    Status = r.Status,
                    RoomId = r.RoomId
                }).ToList(),

                Reviews = r.Reviews.Select(r => new ReviewChildDto()
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    RoomId = r.RoomId,
                    Rating = r.Rating,
                    Comment = r.Comment
                }).ToList(),

                Images = r.Images.Select(i => new ImageChildDto()
                {
                    Id = i.Id,
                    RoomId = i.RoomId,
                    ImageUrl = i.ImageUrl
                }).ToList(),

            });

            return RoomDtos;
        }
    }
}
