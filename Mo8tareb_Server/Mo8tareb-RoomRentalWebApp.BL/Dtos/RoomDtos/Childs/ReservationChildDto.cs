using Mo8tareb_RoomRentalWebApp.DAL.Models;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos.Childs
{
    public class ReservationChildDto
    {
        public int Id { get; set; }

        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }

        public int? RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ReservationStatus Status { get; set; }
    }
}
