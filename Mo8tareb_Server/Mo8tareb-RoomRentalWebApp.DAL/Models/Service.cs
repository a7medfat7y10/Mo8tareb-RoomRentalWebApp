using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.DAL.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<RoomService> RoomServices { get; set; }
    }
}
