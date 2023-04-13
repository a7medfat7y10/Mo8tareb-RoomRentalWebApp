using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.Accounts
{
    public record RegisterModel(string FirstName,
                                string LastName,
                                string UserName,
                                string Email,
                                string Password
                               );
    //public class RegisterModel
    //{
    //    public required string FirstName { get; set; }
    //    public required string LastName { get; set; }
    //    public required string Username { get; set; }
    //    public required string Email { get; set; }
    //    public required string Password { get; set; }
    //}
}
