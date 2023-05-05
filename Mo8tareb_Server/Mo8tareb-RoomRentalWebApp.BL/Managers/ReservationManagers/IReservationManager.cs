using Mo8tareb_RoomRentalWebApp.BL.Dtos._ٌReservationsDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.ReservationsDtos;
﻿using Mo8tareb_RoomRentalWebApp.BL.Dtos.ReservationsDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Managers.ReservationManagers
{
    public interface IReservationManager
    {
        bool DidThisUserReserveThisRoomManager(AppUser user, RoomReadDto room);
        bool DidThisUserReserveThisRoomAndGetApprovedByOwnerManager(AppUser user, RoomReadDto room);
        bool DidThisUserReserveThisRoomAndGetRejectedByOwnerManager(AppUser user, RoomReadDto room);
        bool DidThisUserReserveThisRoomAndGetSuspendedByOwnerManager(AppUser user, RoomReadDto room);

        public Task<ReservationsUpdateDtos?>? UpdateReservationStatus(ReservationsUpdateDtos Reservation);



        Task<IQueryable<ReservationsReadDtos>> GetAllReservationsWithUsersWithRoomsAsync();
        Task<ReservationsCreateDtos?>? CreateReservationWithUsersWithRoomsAsync(ReservationsCreateDtos? createReservationDto);
        Task<ReservationsUpdateDtos?>? UpdateReservationAsync(ReservationsUpdateDtos Reservation);
        Task<ReservationsToDeleteDtos?>? DeleteReservationAsync(ReservationsToDeleteDtos Reservation);
        Task<List<UserReservationDto>> GetConfirmedUserReservationsByUserId(string userId);
        Task<List<UserReservationDto>> GetConfirmedUserReservationsByUserEmail(string mail);
    }
}
