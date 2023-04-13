using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.DAL.Models
{
    public class Image
    {
        public int Id { get; set; }

        public int RoomId { get; set; }
        public virtual Room Room { get; set; }

        public string ImageUrl { get; set; }
    }
}
