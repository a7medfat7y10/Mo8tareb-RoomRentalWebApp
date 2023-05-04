using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.UserDtos
{
    public record userReadDtos(string Email, string firstName,string lastName);


    public class userReadDtosV2
    {
        public string? Email { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
    }
    public record userCreateDtos(string Email);


}
