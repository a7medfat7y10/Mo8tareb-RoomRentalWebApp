using Mo8tareb_RoomRentalWebApp.DAL.Models;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos.Childs
{
    public class ReviewChildDto
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }
        public int? RoomId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}