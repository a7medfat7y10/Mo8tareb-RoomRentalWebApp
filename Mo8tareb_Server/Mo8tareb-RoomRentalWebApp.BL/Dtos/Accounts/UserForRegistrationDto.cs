using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.Accounts
{
    public record UserForRegistrationDto(string FirstName,
                                         string LastName,
                                         string UserName,
                                         string Gender,
                                         string Email,
                                         string phone,
                                         string Password,
                                         string ConfirmPassword,
                                         string ClientURI
                                        );









    //    public class UserForRegistrationDto
    //{
    //    public required string FirstName { get; set; }
    //    public required string LastName { get; set; }
    //    public required string Email { get; set; }
    //    public required string Password { get; set; }
    //    public required string ConfirmPassword { get; set; }
    //    public string ClientURI { get; set; }
    //}
}
