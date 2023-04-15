using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.OwnerDtos;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos
{
    public record RoomReadDtos(int id, string RoomType,OwnerReadDtos Owner);
    public record RoomCreateDtos(int id);
    
}
