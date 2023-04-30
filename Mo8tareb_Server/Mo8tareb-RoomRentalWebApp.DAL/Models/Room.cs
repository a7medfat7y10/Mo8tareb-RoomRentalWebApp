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
        public string RoomDescription { get; set; }
        public required string RoomType { get; set; }
        public required string Location { get; set; }
        public decimal Price { get; set; }
        public bool IsReserved { get; set; }
        public int BedNo { get; set; }
        public string? OwnerId { get; set; }
        public virtual Owner? Owner { get; set; }

        public virtual ICollection<Review>? Reviews { get; set; } = new HashSet<Review>();
        public virtual ICollection<Reservation>? Reservations { get; set; } = new HashSet<Reservation>();
        public virtual ICollection<Service>? Services { get; set; } = new HashSet<Service>();
        public virtual ICollection<Image>? Images { get; set; } = new HashSet<Image>();
    }
}
