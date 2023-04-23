using Microsoft.AspNetCore.Identity;
using Mo8tareb_RoomRentalWebApp.BL.Dtos._ٌReviewsDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.OwnerDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.ReservationsDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos;

using Mo8tareb_RoomRentalWebApp.BL.Dtos.UserDtos;
using Mo8tareb_RoomRentalWebApp.DAL;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System.Linq;

namespace Mo8tareb_RoomRentalWebApp.BL.Managers.ReservationManagers
{
    public class ReservationManager : IReservationManager
    {
        public readonly IUnitOfWork _UnitOfWork;
        public readonly UserManager<AppUser> _userManager;
        public ReservationManager(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _UnitOfWork = unitOfWork;
            _userManager = userManager;
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

            Reservation CreatedReservation = new Reservation()
            {
                StartDate = createReservationDto.StartDate,
                EndDate = createReservationDto.EndDate,
                Status = createReservationDto.Status,
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

    }
}
