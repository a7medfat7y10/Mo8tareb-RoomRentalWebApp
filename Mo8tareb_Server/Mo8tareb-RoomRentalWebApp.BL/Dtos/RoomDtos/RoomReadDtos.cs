using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.OwnerDtos;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos
{
    public record RoomReadDtos(int id, string RoomType, OwnerReadDtos Owner);
    public record RoomCreateDtos(int id);
    //public record RoomDto(int id, string RoomType, decimal Price, string Location, string OwnerId);
    public record RoomDto(int Id, string RoomType, decimal Price, string Location, string OwnerId, int NumOfBeds);
    public record RoomDeleteDto(int id);

}
