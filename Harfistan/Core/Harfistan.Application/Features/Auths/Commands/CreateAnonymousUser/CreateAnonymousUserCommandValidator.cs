using FluentValidation;

namespace Harfistan.Application.Features.Auths.Commands.CreateAnonymousUser;

public class CreateAnonymousUserCommandValidator : AbstractValidator<CreateAnonymousUserCommand>
{
    public CreateAnonymousUserCommandValidator()
    {
        RuleFor(x => x.DeviceId)
            .NotEmpty().WithMessage("DeviceId is required.")
            .MinimumLength(10).WithMessage("DeviceId must be at least 10 characters long.")
            .MaximumLength(255).WithMessage("DeviceId must be at most 255 characters long.");   
    }
}