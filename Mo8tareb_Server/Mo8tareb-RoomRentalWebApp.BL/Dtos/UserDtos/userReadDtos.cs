using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.UserDtos
{
    public record userReadDtos(string Email, string firstName,string lastName);
    public record userCreateDtos(string Email);

}
