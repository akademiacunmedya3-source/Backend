using FluentValidation;

namespace Harfistan.Application.Features.Auths.Commands.LoginWithGoogle;

public class LoginWithGoogleCommandValidator : AbstractValidator<LoginWithGoogleCommand>
{
    public LoginWithGoogleCommandValidator()
    {
        RuleFor(x => x.GoogleId)
            .NotEmpty().WithMessage("GoogleId is required.");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid.");
    }   
}