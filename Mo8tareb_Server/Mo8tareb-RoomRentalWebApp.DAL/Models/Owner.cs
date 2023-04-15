using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.DAL.Models
{
    public class Owner:AppUser
    {
        public virtual ICollection<Room>? Rooms { get; set; } = new HashSet<Room>();
    }
}
