using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mo8tareb_RoomRentalWebApp.BL.Dtos._ٌReservationsDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos._ٌReviewsDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.OwnerDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.ReservationsDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos;

using Mo8tareb_RoomRentalWebApp.BL.Dtos.UserDtos;
using Mo8tareb_RoomRentalWebApp.DAL;
using Mo8tareb_RoomRentalWebApp.DAL.Context;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System.Linq;

namespace Mo8tareb_RoomRentalWebApp.BL.Managers.ReservationManagers
{
    public class ReservationManager : IReservationManager
    {
        public readonly IUnitOfWork _UnitOfWork;
        public readonly UserManager<AppUser> _userManager;
        protected readonly ApplicationDbContext _context;

        public ReservationManager(ApplicationDbContext context, IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _UnitOfWork = unitOfWork;
            _userManager = userManager;
            _context = context;
        }
       
        public async Task<IQueryable<ReservationsReadDtos>> GetAllReservationsWithUsersWithRoomsAsync()
        {
            IQueryable<Reservation>? ReservationsList = await _UnitOfWork.Reservations.GetAllReservationsWithUsersWithRoomsRepoFuncAsync();
            IQueryable<ReservationsReadDtos>? ReservationsDtos = ReservationsList.Select(r =>
            new ReservationsReadDtos
            (
                 r.Id,
                 r.StartDate,
                 r.EndDate,
                 r.Status,
                 new userReadDtos(r.User!.Email!, r.User.FirstName, r.User.LastName),
                 new RoomReadDtos(r.Room!.Id, r.Room.RoomType, new OwnerReadDtos(r.Room!.Owner!.Email!, r.Room.Owner.FirstName, r.Room.Owner.LastName))
            ));
            return ReservationsDtos;
        }
        public async Task<ReservationsCreateDtos?>? CreateReservationWithUsersWithRoomsAsync(ReservationsCreateDtos? createReservationDto)
        {

            AppUser? user = await _userManager.FindByEmailAsync(createReservationDto?.User.Email ?? "");


            if (user is null || createReservationDto is null)
                return null;

            // Check if room is reserved previously

            var room = await _UnitOfWork.Rooms.FindByCondtion(i => i.Id == createReservationDto.Room.id).FirstOrDefaultAsync();
            if (room is null || room.IsReserved)
                return null;

            Reservation CreatedReservation = new Reservation()
            {
                StartDate = Convert.ToDateTime( createReservationDto.StartDate),
                EndDate = Convert.ToDateTime(createReservationDto.EndDate),
                Status =  createReservationDto.Status,
                UserId = user.Id,
                RoomId = createReservationDto.Room.id
            };

            await _UnitOfWork.Reservations.AddAsync(CreatedReservation);

            int rowsAffected = await _UnitOfWork.SaveAsync();

            return rowsAffected > 0 ?
               createReservationDto : null;
        }
        public async Task<ReservationsUpdateDtos?>? UpdateReservationAsync(ReservationsUpdateDtos Reservation)
        {
            Reservation? ReservationFromDatabase = _UnitOfWork.Reservations.FindByCondtion(r => r.Id == Reservation.id).FirstOrDefault();

            if (ReservationFromDatabase is null)
                return null;

            ReservationFromDatabase.StartDate = Reservation.StartDate;
            ReservationFromDatabase.EndDate = Reservation.EndDate;
            ReservationFromDatabase.Status = Reservation.Status;

            _UnitOfWork.Reservations.Update(ReservationFromDatabase);

            int rowsAffected = await _UnitOfWork.SaveAsync();
            return rowsAffected > 0 ? Reservation : null;

        }
        public async Task<ReservationsToDeleteDtos?>? DeleteReservationAsync(ReservationsToDeleteDtos Reservation)
        {
            Reservation? ReservationFromDatabase = _UnitOfWork.Reservations.FindByCondtion(r => r.Id == Reservation.id).FirstOrDefault();

            if (ReservationFromDatabase is null)
                return null;

            _UnitOfWork.Reservations.Remove(ReservationFromDatabase);

            int rowsAffected = await _UnitOfWork.SaveAsync();
            return rowsAffected > 0 ? Reservation : null;



        }

        public async Task<List<UserReservationDto>> GetConfirmedUserReservationsByUserId(string userId)
        {
            return await _UnitOfWork.Reservations
                .FindByCondtion(i => i.UserId == userId && i.Status == ReservationStatus.Approved)
                .Select(r => new UserReservationDto
                {
                    Id = r.Id,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    RoomId = r.RoomId
                }).ToListAsync();
        }

        public async Task<List<UserReservationDto>> GetConfirmedUserReservationsByUserEmail(string mail)
        {
            return await _UnitOfWork.Reservations
                .FindByCondtion(i => i.UserId != null && i.User.Email == mail && i.Status == ReservationStatus.Approved)
                .Select(r => new UserReservationDto
                {
                    Id = r.Id,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    RoomId = r.RoomId
                }).ToListAsync();
        }

        bool IReservationManager.DidThisUserReserveThisRoomManager(AppUser user, RoomReadDto room)
        {
                var usersList = _context.AppUsers.ToList();
                var reservedRoomList = _context.Reservations.Include(r => r.Room).ToList();

                var reservedRoom= reservedRoomList.FirstOrDefault(r => r.UserId == user.Id && r.RoomId == room.Id);
            if (reservedRoom == null) return false;

            return reservedRoom != null ? true : false;
        }

        public bool DidThisUserReserveThisRoomAndGetApprovedByOwnerManager(AppUser user, RoomReadDto room)
        {
            var usersList = _context.AppUsers.ToList();
            var reservedRoomList = _context.Reservations.Include(r => r.Room).ToList();

            var reservedRoom = reservedRoomList.FirstOrDefault(r => r.UserId == user.Id && r.RoomId == room.Id);
            if (reservedRoom == null) return false;

            return reservedRoom.Status==ReservationStatus.Approved ? true : false;
        }

        public bool DidThisUserReserveThisRoomAndGetRejectedByOwnerManager(AppUser user, RoomReadDto room)
        {
            var usersList = _context.AppUsers.ToList();
            var reservedRoomList = _context.Reservations.Include(r => r.Room).ToList();

            var reservedRoom = reservedRoomList.FirstOrDefault(r => r.UserId == user.Id && r.RoomId == room.Id);
            if (reservedRoom == null) return false;

            return reservedRoom.Status == ReservationStatus.Rejected ? true : false;
        }

        public bool DidThisUserReserveThisRoomAndGetSuspendedByOwnerManager(AppUser user, RoomReadDto room)
        {
            var usersList = _context.AppUsers.ToList();
            var reservedRoomList = _context.Reservations.Include(r => r.Room).ToList();

            var reservedRoom = reservedRoomList.FirstOrDefault(r => r.UserId == user.Id && r.RoomId == room.Id);
            if (reservedRoom == null) return false;

            return reservedRoom.Status == ReservationStatus.Pending ? true : false;
        }


        
        public async Task<ReservationsUpdateDtos?>? UpdateReservationStatus(ReservationsUpdateDtos Reservation)
        {
            Reservation? ReservationFromDatabase = _UnitOfWork.Reservations.FindByCondtion(r => r.Id == Reservation.id).FirstOrDefault();

            if (ReservationFromDatabase is null)
                return null;

            ReservationFromDatabase.Status = Reservation.Status;

            _UnitOfWork.Reservations.Update(ReservationFromDatabase);

            int rowsAffected = await _UnitOfWork.SaveAsync();
            return rowsAffected > 0 ? Reservation : null;

        }

    }
}
