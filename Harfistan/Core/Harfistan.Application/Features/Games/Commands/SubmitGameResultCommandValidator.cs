using FluentValidation;

namespace Harfistan.Application.Features.Games.Commands;

public class SubmitGameResultCommandValidator : AbstractValidator<SubmitGameResultCommand>
{
    public SubmitGameResultCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
        RuleFor(x => x.Attempts).InclusiveBetween(1,6).WithMessage("Attempts must be between 1 and 6.");
        RuleFor(x => x.DurationSeconds).GreaterThan(0).WithMessage("Duration must be greater than 0.");
        RuleFor(x=> x.Guesses).NotEmpty().WithMessage("Guesses is required.")
            .Must((cmd, guesses) => guesses.Count == cmd.Attempts).WithMessage("Number of guesses must be equal to Attempts.");
        RuleFor(x => x.Guesses)
            .Must(guesses => guesses.All(g => !string.IsNullOrWhiteSpace(g))).WithMessage("All guesses must be valid words");
    }
}