using FluentValidation;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.PaymentDtos;
using System.ComponentModel.DataAnnotations;

namespace Mo8tareb_RoomRentalWebApp.Api.Validators
{
    public class CreateCheckoutSessionRequestValidator : AbstractValidator<CreateCheckoutSessionRequest>
    {
        public CreateCheckoutSessionRequestValidator()
        {
            RuleFor(i => i.RoomPrice)
                .Must(p => p > 0)
                .WithMessage("Room price must be larger than zero");

            RuleFor(i => i.SuccessUrl)
                .NotNull()
                .NotEmpty()
                .WithMessage("Success url must be provided");

            RuleFor(i => i.FailureUrl)
                .NotNull()
                .NotEmpty()
                .WithMessage("Failure url must be provided");
        }
    }
}
