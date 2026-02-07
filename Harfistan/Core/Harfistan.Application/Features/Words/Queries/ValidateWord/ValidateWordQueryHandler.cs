using Harfistan.Application.Abstractions.Repositories;
using Harfistan.Application.DTOs.Words;
using Mediator;

namespace Harfistan.Application.Features.Words.Queries.ValidateWord;

public record ValidateWordQuery(string Word) : IRequest<WordValidationDTO>;

public class ValidateWordQueryHandler(IWordRepository wordRepository) : IRequestHandler<ValidateWordQuery, WordValidationDTO>
{
    public async ValueTask<WordValidationDTO> Handle(ValidateWordQuery request, CancellationToken cancellationToken)
    {
        var normalizedWord = request.Word.ToUpperInvariant().Trim();
        var exists = await wordRepository.ExistsAsync(request.Word, cancellationToken);
        return new WordValidationDTO()
        {
            IsValid = exists,
            Word = normalizedWord,
            Message = exists ? "Word already exists" : "Word not found in dictionary"
        };
    }
}