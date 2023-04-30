using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos
{
    public class RoomChildDto
    {
        public int Id { get; set; }
        public required string RoomType { get; set; }
        public required string Location { get; set; }
        public decimal Price { get; set; }

        public bool IsReserved { get; set; }
        public int BedNo { get; set; }
    }
        
    public record EmailRoomIdDto(string userEmail, int RoomId);
}
