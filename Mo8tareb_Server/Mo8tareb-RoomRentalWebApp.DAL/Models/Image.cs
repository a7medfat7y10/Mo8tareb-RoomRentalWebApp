using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.DAL.Models
{
    public class Image
    {
        public int Id { get; set; }

        public int? RoomId { get; set; }
        public virtual Room? Room { get; set; }
        [MaxLength]
        public  byte[] ImageUrl { get; set; }

    }
}
