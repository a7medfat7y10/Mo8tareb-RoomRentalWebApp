using Microsoft.AspNetCore.Identity;
using Mo8tareb_RoomRentalWebApp.DAL.Constants;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.DAL.Models
{
    public class AppUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        [EnumDataType(typeof(Enums.Gender), ErrorMessage = "you should enter valid Enum data type(Male or Female)")]
        public Enums.Gender Gender { get; set; }
        public virtual ICollection<Reservation>? Reservations { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}
