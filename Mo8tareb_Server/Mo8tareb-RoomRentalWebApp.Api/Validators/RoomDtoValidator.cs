using FluentValidation;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.RoomDtos;

namespace Mo8tareb_RoomRentalWebApp.Api.Validators
{
    public class RoomDtoValidator : AbstractValidator<RoomDto>
    {
        public RoomDtoValidator()
        {
            RuleFor(i => i.Price)
                .Must(p => p > 0)
                .WithMessage("Room price must be more than zero");

            RuleFor(i => i.NumOfBeds)
                .Must(p => p > 0)
                .WithMessage("Room must have at least one bed");

        }
    }
}
