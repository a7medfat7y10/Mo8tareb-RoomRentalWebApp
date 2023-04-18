using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.ServieDtos
{
    public class ServiceReadDtos
    {

        //public ServiceReadDtos(int id, string name, ICollection<RoomChildDto>? rooms)
        //{
        //    Id = id;
        //    Name = name;
        //    this.Rooms = rooms;
        //}

        public int Id { get; set; }
        public required string Name { get; set; }

        public virtual ICollection<RoomChildDto>? Rooms { get; set; } = new HashSet<RoomChildDto>();
    }
}
