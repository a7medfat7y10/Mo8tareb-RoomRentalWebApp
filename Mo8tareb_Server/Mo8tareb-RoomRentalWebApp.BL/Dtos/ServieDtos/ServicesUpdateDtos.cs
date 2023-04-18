using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.UserDtos;
using System.ComponentModel.DataAnnotations;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.ServieDtos
{
        public record ServicesCreateDtos(string Name);
        public record ServicesUpdateDtos(int id, string Name);
}