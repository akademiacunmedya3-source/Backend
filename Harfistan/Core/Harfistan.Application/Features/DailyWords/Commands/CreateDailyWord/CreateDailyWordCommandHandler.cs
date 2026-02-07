using System.Security.Cryptography;
using System.Text;
using Harfistan.Application.Abstractions.Repositories;
using Harfistan.Application.Exceptions;
using Harfistan.Domain.Entities;
using Mediator;

namespace Harfistan.Application.Features.DailyWords.Commands.CreateDailyWord;

public record CreateDailyWordCommand(DateTime Date, int? WordId, int WordLength) : IRequest<int>;

public class CreateDailyWordCommandHandler(IDailyWordRepository dailyWordRepository, IWordRepository wordRepository) : IRequestHandler<CreateDailyWordCommand, int>
{
    public async ValueTask<int> Handle(CreateDailyWordCommand request, CancellationToken cancellationToken)
    {
        var existingDailyWord = await dailyWordRepository.GetByDateAsync(request.Date.Date, cancellationToken);
        if (existingDailyWord is not null)
            throw new AlreadyExistsException($"DailyWord", request.Date.Date);

        Word word;
        if (request.WordId.HasValue)
        {
            word = await wordRepository.GetByIdAsync(request.WordId.Value, cancellationToken) ?? throw new NotFoundException($"Word", request.WordId.Value);
        }
        else
        {
            word = await wordRepository.GetRandomWordAsync(request.WordLength, cancellationToken);

            if (word is null)
                throw new NotFoundException($"No common words found with length {request.WordLength}");
        }
        
        var wordHash = GenerateWordHash(word.Text);
        var dailyWord = new DailyWord
        {
            Date = request.Date.Date,
            WordId = word.Id,
            WordHash = wordHash,
            CreatedAt = DateTime.UtcNow
        };
        
        await dailyWordRepository.AddAsync(dailyWord, cancellationToken);
        await dailyWordRepository.SaveChangesAsync(cancellationToken);
        
        return dailyWord.Id;
    }
    
    private static string GenerateWordHash(string word)
    {
        using var md5 = MD5.Create();
        var inputBytes = Encoding.UTF8.GetBytes(word.ToUpperInvariant());
        var hashBytes = md5.ComputeHash(inputBytes);
        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }
}