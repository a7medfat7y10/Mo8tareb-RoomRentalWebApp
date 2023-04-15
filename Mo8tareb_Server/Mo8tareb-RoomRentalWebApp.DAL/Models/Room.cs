using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.DAL.Models
{
    public class Room
    {
        public int Id { get; set; }
        public required string RoomType { get; set; }
        public required string Location { get; set; }
        
        public decimal Price { get; set; }

        public string? OwnerId { get; set; }
        public virtual Owner? Owner { get; set; }

        public virtual ICollection<Review>? Reviews { get; set; }
        public virtual ICollection<Reservation>? Reservations { get; set; }
        public virtual ICollection<RoomService>? RoomServices { get; set; }
        public virtual ICollection<Image>? Images { get; set; }
    }
}
