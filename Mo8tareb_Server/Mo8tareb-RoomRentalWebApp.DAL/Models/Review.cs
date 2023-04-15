using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.DAL.Models
{
    public class Review
    {
        public int Id { get; set; }

        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }

        public int? RoomId { get; set; }
        public virtual Room? Room { get; set; }

        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
