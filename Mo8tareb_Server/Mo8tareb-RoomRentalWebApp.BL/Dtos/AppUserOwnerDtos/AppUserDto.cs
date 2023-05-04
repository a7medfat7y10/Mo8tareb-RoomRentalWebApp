using Mo8tareb_RoomRentalWebApp.BL.Dtos._ٌReviewsDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.PaymentDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.ReservationsDtos;
using Mo8tareb_RoomRentalWebApp.DAL.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.AppUserOwnerDtos
{
    public class AppUserDto
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public Enums.Gender Gender { get; set; }
        public ICollection<ReservationsReadDtosV2>? Reservations { get; set; }
        public ICollection<ReviewsReadDtosV2>? Reviews { get; set; }
        public ICollection<PaymentReadDto>? Payments { get; set; }
    }
}
