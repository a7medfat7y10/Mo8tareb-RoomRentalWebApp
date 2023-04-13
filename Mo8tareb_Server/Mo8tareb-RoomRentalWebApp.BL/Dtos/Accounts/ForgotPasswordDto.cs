using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.Accounts
{
    public record ForgotPasswordDto(string Email, string ClientURI);
    //public class ForgotPasswordDto { 
    //    [EmailAddress]
    //    public required string Email { get; set; }
    //    public required string ClientURI { get; set; }
    //}
}
