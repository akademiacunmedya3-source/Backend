using FluentValidation;

namespace Harfistan.Application.Features.DailyWords.Commands.CreateDailyWord;

public class CreateDailyWordCommandValidator : AbstractValidator<CreateDailyWordCommand>
{
    public CreateDailyWordCommandValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.")
            .Must(date => date.Date >= DateTime.UtcNow.Date).WithMessage("Can not create daily word for past dates");   
        
        RuleFor(x => x.WordLength)
            .InclusiveBetween(4,10).WithMessage("Word length must be between 4 and 10 characters");
    }
}