using FluentValidation;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.Accounts;

namespace Mo8tareb_RoomRentalWebApp.Api.Validators
{
    //public record UserForAuthenticationDto(string Email, string Password, string clientURI);
    //// public record AuthResponseDto(bool IsAuthSuccessful, string ErrorMessage, string Token);


    ////public class UserForAuthenticationDto
    ////{
    ////    [Required(ErrorMessage = "Email is required.")]
    ////    public string Email { get; set; }
    ////    [Required(ErrorMessage = "Password is required.")]
    ////    public string Password { get; set; }
    ////    public string clientURI { get; set; }
    ////}


    //public class AuthResponseDto
    //{
    //    public bool IsAuthSuccessful { get; set; }
    //    public string? ErrorMessage { get; set; }
    //    public string? Token { get; set; }
    //    public string? Email { get; set; }
    //    public string? Role { get; set; }





        public class UserForAuthenticationDtoValidation : AbstractValidator<UserForAuthenticationDto>
    {
        public UserForAuthenticationDtoValidation()
        {
            RuleFor(i => i.Email)
              .NotEmpty()
              .WithMessage("the Email must not be empty");

            RuleFor(i => i.Password)
                .NotEmpty()
                .WithMessage("the password must not be empty");

        }

    }
}
