using FluentValidation;

namespace Harfistan.Application.Features.Words.Queries.ValidateWord;

public class ValidateWordQueryValidator : AbstractValidator<ValidateWordQuery>
{
    public ValidateWordQueryValidator()
    {
        RuleFor(x => x.Word)
            .NotEmpty().WithMessage("Word is required.")
            .MinimumLength(3).WithMessage("Word must be at least 3 characters long.")
            .MaximumLength(20).WithMessage("Word must be at most 20 characters long.")
            .Matches("^[A-ZÇĞİÖŞÜ]+$").WithMessage("Word must contain only Turkish uppercase letters");
    }
}