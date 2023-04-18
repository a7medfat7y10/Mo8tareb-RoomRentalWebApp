using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos.Childs
{
    public class ImageChildDto
    {
        public int Id { get; set; }
        public int? RoomId { get; set; }

        [MaxLength]
        public byte[] ImageUrl { get; set; }
    }
}