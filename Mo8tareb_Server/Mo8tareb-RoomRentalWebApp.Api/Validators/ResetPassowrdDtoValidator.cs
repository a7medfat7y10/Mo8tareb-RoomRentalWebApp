using FluentValidation;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.Accounts;

namespace Mo8tareb_RoomRentalWebApp.Api.Validators
{
    public class ResetPassowrdDtoValidator :AbstractValidator<ResetPasswordDto>
    {
        public ResetPassowrdDtoValidator()
        {
            //public record ResetPasswordDto(string Password,
            //                       string ConfirmPassword,
            //                       string Email,
            //                       string? Token
            //                      );

            RuleFor(x => x.Password)
                .NotEmpty()
                .NotNull()
                .WithMessage("new password must be provided");
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .NotNull()
                .WithMessage("confirming password must be provided");
            RuleFor(x => x.Email)
                .NotEmpty()
                .NotNull()
                .WithMessage("Email must be provided");

        }

    }
}
