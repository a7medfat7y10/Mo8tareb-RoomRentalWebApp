using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.Accounts
{
    public record ResetPasswordDto(string Password,
                                   string ConfirmPassword,
                                   string Email,
                                   string? Token
                                  );
    //public class ResetPasswordDto
    //{
    //    public required string Password { get; set; }

    //    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    //    public required string ConfirmPassword { get; set; }

    //    public  string? Email { get; set; }
    //    public  string? Token { get; set; }
    //}
}
