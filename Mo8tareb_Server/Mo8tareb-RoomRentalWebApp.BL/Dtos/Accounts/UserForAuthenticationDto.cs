using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mo8tareb_RoomRentalWebApp.BL.Dtos.Accounts
{
    public record UserForAuthenticationDto(string Email, string Password, string clientURI);
   // public record AuthResponseDto(bool IsAuthSuccessful, string ErrorMessage, string Token);


    //public class UserForAuthenticationDto
    //{
    //    [Required(ErrorMessage = "Email is required.")]
    //    public string Email { get; set; }
    //    [Required(ErrorMessage = "Password is required.")]
    //    public string Password { get; set; }
    //    public string clientURI { get; set; }
    //}


    public class AuthResponseDto
    {
        public bool IsAuthSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Token { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}
