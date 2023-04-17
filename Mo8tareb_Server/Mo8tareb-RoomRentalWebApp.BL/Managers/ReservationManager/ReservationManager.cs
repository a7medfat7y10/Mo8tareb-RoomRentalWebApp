using AutoMapper;
using Mo8tareb_RoomRentalWebApp.Api.Payloads;
using Mo8tareb_RoomRentalWebApp.DAL;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Managers.ReservationManager
{
    public class ReservationManager : IReservationManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReservationManager(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Reservation> CreateReservationAsync(ReservationPayload payload, string userId)
        {
            //var reservation = _mapper.Map<Reservation>(payload); TODO: Fix mapper
            var reservation = new Reservation
            {
                RoomId = 1,
                StartDate = payload.StartDate,
                EndDate = payload.EndDate,
                Status = ReservationStatus.Pending

            };
            await _unitOfWork.ReservationRepo.AddAsync(reservation);
            await _unitOfWork.SaveAsync();
            return reservation;
        }
    }
}
