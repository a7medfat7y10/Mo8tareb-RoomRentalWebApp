using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos.Childs
{
   // public record RoomLocationsDto(int RoomId, string Location);

    public class RoomLocationsDto
    {
        public int RoomId { get; set; }
        public string? Location { get; set; }
    }
}
