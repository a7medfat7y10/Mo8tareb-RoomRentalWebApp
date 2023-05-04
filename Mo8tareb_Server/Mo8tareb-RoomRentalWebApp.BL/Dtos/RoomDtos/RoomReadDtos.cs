using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.OwnerDtos;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos
{
    public record RoomReadDtos(int id, string RoomType, OwnerReadDtos Owner);
    public class RoomReadDtosV2
    {
        public int id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public long Price { get; set; }
        public string? RoomType { get; set; }
        public OwnerReadDtosV2? Owner { get; set; }
    }
    public record RoomCreateDtos(int id);
    public record RoomCreateDto(int id, string RoomType, decimal Price, string Location, string OwnerId);
    public record RoomUpdateDto(int id, string RoomType, decimal Price, string Location, string OwnerId);

    //public record RoomDto(int id, string RoomType, decimal Price, string Location, string OwnerId);
    public record RoomDto(int Id, string RoomType, decimal Price, string Location, string OwnerId, string Description, int NumOfBeds,bool isreserved);
    public record RoomDeleteDto(int id);

}
