using Microsoft.AspNetCore.Identity;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.ReservationsDtos;
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
using Microsoft.AspNetCore.Identity;

namespace Mo8tareb_RoomRentalWebApp.BL.Managers.RoomManagers
{
    public class RoomManager: IRoomManager
    {
        public RoomManager(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _UnitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IUnitOfWork _UnitOfWork { get; }
        public UserManager<AppUser> _userManager { get; }

        public async Task<IQueryable<RoomReadDto>> GetAllRoomsWithDetails()
        {
            var rooms = await _UnitOfWork.Rooms.GetAllRoomsWithAllDetails();

            var RoomDtos = rooms.Select(r =>
            new RoomReadDto()
            {
                Id = r.Id,
                NoBeds=r.BedNo,
                Description=r.RoomDescription,
                IsReserved=r.IsReserved,
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

        public async Task<RoomReadDto> GetRoomWithDetails(int id)
        {
            Room? RoomFromDatabase = await _UnitOfWork.Rooms.GetRoom(id);

            if (RoomFromDatabase == null)
                return null;

            var RoomDto = new RoomReadDto()
            {
                Id = RoomFromDatabase.Id,
                NoBeds=RoomFromDatabase.BedNo,
                Description=RoomFromDatabase.RoomDescription,
                IsReserved=RoomFromDatabase.IsReserved,
                Price = RoomFromDatabase.Price,
                Location = RoomFromDatabase.Location,
                RoomType = RoomFromDatabase.RoomType,
                OwnerId = RoomFromDatabase.OwnerId,
                Services = RoomFromDatabase.Services.Select(s => new ServiceChildDto()
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList(),

                Reservations = RoomFromDatabase.Reservations.Select(r => new ReservationChildDto()
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    Status = r.Status,
                    RoomId = r.RoomId
                }).ToList(),

                Reviews = RoomFromDatabase.Reviews.Select(r => new ReviewChildDto()
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    RoomId = r.RoomId,
                    Rating = r.Rating,
                    Comment = r.Comment
                }).ToList(),

                Images = RoomFromDatabase.Images.Select(i => new ImageChildDto()
                {
                    Id = i.Id,
                    RoomId = i.RoomId,
                    ImageUrl = i.ImageUrl
                }).ToList(),

            };

            return RoomDto;
        }

        public async Task<Room?>? CreateRoomsAsync(RoomDto? createRoomDto)
        {
            if (createRoomDto == null)//|| roomId==null
                return null;

            Room CreatedRoom = new Room()
            {
                Location = createRoomDto.Location,
                RoomType = createRoomDto.RoomType,
                OwnerId = createRoomDto.OwnerId,
                Price = createRoomDto.Price,
                BedNo = createRoomDto.NumOfBeds,
                RoomDescription=createRoomDto.Description,
                IsReserved=createRoomDto.isreserved
            };

            await _UnitOfWork.Rooms.AddAsync(CreatedRoom);
            int rowsAffected = await _UnitOfWork.SaveAsync();

            return rowsAffected > 0 ?
               CreatedRoom : null;
        }
        public async Task<RoomDto?>? UpdateRoomAsync(RoomDto room)
        {
            Room? RoomFromDatabase = _UnitOfWork.Rooms.FindByCondtion(r => r.Id == room.Id).FirstOrDefault();

            if (RoomFromDatabase == null)
                return null;

            RoomFromDatabase.Location = room.Location;
            RoomFromDatabase.RoomType = room.RoomType;
            RoomFromDatabase.OwnerId = room.OwnerId;
            RoomFromDatabase.Price = room.Price;
            RoomFromDatabase.BedNo = room.NumOfBeds;
            RoomFromDatabase.IsReserved = room.isreserved;
            RoomFromDatabase.RoomDescription = room.Description;

            _UnitOfWork.Rooms.Update(RoomFromDatabase);

            int rowsAffected = await _UnitOfWork.SaveAsync();
            return rowsAffected > 0 ? room : null;
        }
        public async Task<RoomDeleteDto?>? DeleteRoomAsync(RoomDeleteDto room)
        {
            Room? RoomFromDatabase = _UnitOfWork.Rooms.FindByCondtion(r => r.Id == room.id).FirstOrDefault();

            if (RoomFromDatabase == null)
                return null;

            _UnitOfWork.Rooms.Remove(RoomFromDatabase);

            int rowsAffected = await _UnitOfWork.SaveAsync();
            return rowsAffected > 0 ? room : null;

        }

        public async Task<IQueryable<RoomLocationsDto>> GetRoomsLocations()
        {
            IQueryable<Room>? rooms = await _UnitOfWork.Rooms.GetAllAsync();

            if (rooms is null || !rooms.Any())
                return null!;

            IQueryable<RoomLocationsDto>? roomLocations = rooms.Select(r => new RoomLocationsDto
            {
                RoomId = r.Id,
                Location = r.Location
            }).
            Distinct();

            return roomLocations.AsQueryable();
        }

        public async Task<IQueryable<RoomReadDto>> GetRoomsByLocation(string location)
        {
            IQueryable<Room> rooms = await _UnitOfWork.Rooms.GetAllAsync();

            if (rooms is null || !rooms.Any()) return null!;

            IQueryable<RoomReadDto> roomDtos = rooms
                .Where(r => r.Location == location)
                .Select(r => new RoomReadDto
                {
                    Id = r.Id,
                    RoomType = r.RoomType,
                    Location = r.Location,
                    Price = r.Price,
                    OwnerId = r.OwnerId,
                    Description=r.RoomDescription,
                    IsReserved=r.IsReserved,
                    NoBeds=r.BedNo,
                    Owner = new OwnerChildDto
                    {
                        Id = r.Owner!.Id!,
                        FirstName = r.Owner.FirstName!,
                        LastName = r.Owner.LastName!
                    },
                    Reviews = r.Reviews!.Select(review => new ReviewChildDto
                    {
                        Id = review.Id,
                        Rating = review.Rating,
                        Comment = review.Comment
                    }).ToList(),
                    Reservations = r.Reservations!.Select(reservation => new ReservationChildDto
                    {
                        Id = reservation.Id,
                        StartDate = reservation.StartDate,
                        EndDate = reservation.EndDate,
                        Status = reservation.Status
                    }).ToList(),
                    Services = r.Services!.Select(service => new ServiceChildDto
                    {
                        Id = service.Id,
                        Name = service.Name
                    }).ToList(),
                    Images = r.Images!.Select(image => new ImageChildDto
                    {
                        Id = image.Id,
                        RoomId = image.RoomId,
                        ImageUrl = image.ImageUrl
                    }).ToList()
                });

            return roomDtos;
        }
    }
}
