using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mo8tareb_RoomRentalWebApp.BL.Dtos._ٌReviewsDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.OwnerDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.ReservationsDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos.Childs;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.UserDtos;
using Mo8tareb_RoomRentalWebApp.BL.Managers.RoomManagers;
using Mo8tareb_RoomRentalWebApp.BL.Managers.ServiceManagers;
using Mo8tareb_RoomRentalWebApp.DAL.Context;
using Mo8tareb_RoomRentalWebApp.DAL.Models;

namespace Mo8tareb_RoomRentalWebApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IRoomManager RoomManager;
        private readonly IValidator<RoomDto> _roomDtoValidator;
        private UserManager<AppUser> _userManager;

        public RoomsController(UserManager<AppUser> userManger, ApplicationDbContext context,IRoomManager roomManager, IValidator<RoomDto> roomDtoValidator)
        {
            this.context = context;
            RoomManager = roomManager;
            _roomDtoValidator = roomDtoValidator;
            _userManager = userManger;
        }


        [HttpGet]
        [Route("GetAllRooms")]
        public async Task<IActionResult> GetAllRooms()
        {
            
            var lst = await RoomManager.GetAllRoomsWithDetails();

            return lst.Count() == 0 ? NotFound() : Ok(lst);
        }


        //var rooms = await _UnitOfWork.Rooms.GetAllRoomsWithAllDetails();

        //var RoomDtos = rooms.Select(r =>
        //new RoomReadDto()
        //{
        //    Id = r.Id,
        //    NoBeds = r.BedNo,
        //    Description = r.RoomDescription,
        //    IsReserved = r.IsReserved,
        //    Price = r.Price,
        //    Location = r.Location,
        //    RoomType = r.RoomType,
        //    OwnerId = r.OwnerId,
        //    Services = r.Services.Select(s => new ServiceChildDto()
        //    {
        //        Id = s.Id,
        //        Name = s.Name
        //    }).ToList(),

        //    Reservations = r.Reservations.Select(r => new ReservationChildDto()
        //    {
        //        Id = r.Id,
        //        UserId = r.UserId,
        //        StartDate = r.StartDate,
        //        EndDate = r.EndDate,
        //        Status = r.Status,
        //        RoomId = r.RoomId
        //    }).ToList(),

        //    Reviews = r.Reviews.Select(r => new ReviewChildDto()
        //    {
        //        Id = r.Id,
        //        UserId = r.UserId,
        //        RoomId = r.RoomId,
        //        Rating = r.Rating,
        //        Comment = r.Comment
        //    }).ToList(),

        //    Images = r.Images.Select(i => new ImageChildDto()
        //    {
        //        Id = i.Id,
        //        RoomId = i.RoomId,
        //        ImageUrl = i.ImageUrl
        //    }).ToList(),

        //});

        //    return RoomDtos;


        [HttpGet]
        [Route("GetAllRoomsOfUser")]
        public async Task<IActionResult> GetAllRoomsOfUser(string Email)
        {
            AppUser? user = await _userManager.FindByEmailAsync(Email);

            if (user == null)
                return BadRequest("");

            context.AppUsers.ToList();
            var lst =  context.Reservations.Where(r => r.UserId == user.Id).Select(r =>
            new ReservationsReadDtos
            (
                 r.Id,
                 r.StartDate,
                 r.EndDate,
                 r.Status,
                 new userReadDtos(r.User!.Email!, r.User.FirstName, r.User.LastName),
                 new RoomReadDtos(r.Room!.Id, r.Room.RoomType, new OwnerReadDtos(r.Room!.Owner!.Email!, r.Room.Owner.FirstName, r.Room.Owner.LastName))

            )
                ).ToList();

           var RoomIdLst=lst.Select(r=>r.Room.id).ToList();

            var RoomsOfUser = context.Rooms.Where(r => RoomIdLst.Contains(r.Id))
                .Select(r =>
                new RoomReadDto()
                {
                    Id = r.Id,
                    NoBeds = r.BedNo,
                    Description = r.RoomDescription,
                    IsReserved = r.IsReserved,
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

                }).ToList();

            return RoomsOfUser.Count() == 0 ? NotFound() : Ok(RoomsOfUser);
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
        public async Task<object> CreateRoom(RoomDto room)
        {
            var validationResult = _roomDtoValidator.Validate(room);

            if (!validationResult.IsValid)
                return BadRequest(new { StatusCode = 400, Errors = validationResult.Errors.ToDictionary(i => i.PropertyName, i => i.ErrorMessage) });


            Room? objectCreated = await RoomManager?.CreateRoomsAsync(room)!;

            return objectCreated != null ? objectCreated.Id : BadRequest("Could not create Room due to the inValid data you sent :(");
        }

        [HttpPut]
        [Route("UpdateRoom")]
        public async Task<IActionResult> UpdateRoom(int id, RoomDto room)
        {
            var validationResult = _roomDtoValidator.Validate(room);

            if (!validationResult.IsValid)
                return BadRequest(new { StatusCode = 400, Errors = validationResult.Errors.ToDictionary(i => i.PropertyName, i => i.ErrorMessage) });

            RoomDto? objectUpdated = await RoomManager.UpdateRoomAsync(room)!;

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

        [HttpGet]
        [Route("GetRoomsByLocation")]
        public async Task<IActionResult> GetRoomsByLocation(string location)
        {
            IQueryable<RoomReadDto>? rooms = await RoomManager.GetRoomsByLocation(location);

            return rooms is not null ? Ok(rooms) : NotFound();
        }

        [HttpGet]
        [Route("GetRoomsLocations")]
        public async Task<IActionResult> GetRoomsLocations()
        {
            IQueryable<RoomLocationsDto>? locationsOfRooms = await RoomManager.GetRoomsLocations();
               
            return locationsOfRooms is not null? Ok(locationsOfRooms) : NotFound();
        }
    }
}
