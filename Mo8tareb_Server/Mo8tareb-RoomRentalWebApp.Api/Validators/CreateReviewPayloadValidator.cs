using FluentValidation;
using Mo8tareb_RoomRentalWebApp.BL.Dtos._ٌReviewsDtos;

namespace Mo8tareb_RoomRentalWebApp.BL.Validators
{
    public class CreateReviewPayloadValidator : AbstractValidator<CreateReviewPayload>
    {
        public CreateReviewPayloadValidator()
        {
            RuleFor(i => i.Rating)
                .InclusiveBetween(0, 10)
                .WithMessage("Rating must be between 0 and 10");

            RuleFor(i => i.UserEmail)
                .NotNull()
                .NotEmpty()
                .WithMessage("User id must be provided");

        }
    }
}
