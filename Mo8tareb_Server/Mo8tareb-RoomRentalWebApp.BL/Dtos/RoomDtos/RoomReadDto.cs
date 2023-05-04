using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos.Childs;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos
{
    public class RoomReadDto
    {
        public int Id { get; set; }
        public required string RoomType { get; set; }
        public required string Location { get; set; }
        public required string Description { get; set; }
        public required int NoBeds { get; set; }
        public required bool IsReserved { get; set; }

        public decimal Price { get; set; }

        public string? OwnerId { get; set; }
        public virtual OwnerChildDto? Owner { get; set; }

        public virtual ICollection<ReviewChildDto>? Reviews { get; set; } = new HashSet<ReviewChildDto>();
        public virtual ICollection<ReservationChildDto>? Reservations { get; set; } = new HashSet<ReservationChildDto>();
        public virtual ICollection<ServiceChildDto>? Services { get; set; } = new HashSet<ServiceChildDto>();
        public virtual ICollection<ImageChildDto>? Images { get; set; } = new HashSet<ImageChildDto>();
    }
}
