using FluentValidation;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.Accounts;

namespace Mo8tareb_RoomRentalWebApp.Api.Validators
{
    public class UserForRegistirationDtoValidation:AbstractValidator<UserForRegistrationDto>
    {
        //public record UserForRegistrationDto(string FirstName,
        //                                        string LastName,
        //                                        string UserName,
        //                                        string Gender,
        //                                        string Email,
        //                                        string phone,
        //                                        string Password,
        //                                        string ConfirmPassword,
        //                                        string ClientURI
        //                                       );


        public UserForRegistirationDtoValidation()
        {
            RuleFor(i => i.FirstName)
                .NotNull()
                .NotEmpty()
                .WithMessage("First name is required");

            RuleFor(i => i.LastName)
               .NotNull()
               .NotEmpty()
               .WithMessage("Last name is required");

            RuleFor(i => i.Email)
               .NotNull()
               .NotEmpty()
               .WithMessage("Email is required");

            RuleFor(i => i.Password)
               .NotNull()
               .NotEmpty()
               .WithMessage("Password is required");

            RuleFor(i => i.ConfirmPassword)
               .NotNull()
               .NotEmpty()
               .WithMessage("confirming password is required");

            RuleFor(i => i.ClientURI)
                .NotEmpty()
                .NotNull()
                .WithMessage("Client URL must be provided");
        }
    }
}
