using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.DAL.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }

        public int? RoomId { get; set; }
        public virtual Room? Room { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

        public int? PaymentId { get; set; }
        public virtual Payment? Payment { get; set; }

    }
    public enum ReservationStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
