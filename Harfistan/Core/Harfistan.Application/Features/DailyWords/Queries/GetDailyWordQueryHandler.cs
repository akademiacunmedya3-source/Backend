using Harfistan.Application.Abstractions.Repositories;
using Harfistan.Application.DTOs.DailyWords;
using Mediator;

namespace Harfistan.Application.Features.DailyWords.Queries;

public record GetDailyWordQuery(Guid? UserId, DateTime? Date) : IRequest<DailyWordDTO?>;

public class GetDailyWordQueryHandler(IDailyWordRepository dailyWordRepository, IGameResultRepository gameResultRepository) : IRequestHandler<GetDailyWordQuery, DailyWordDTO>
{
    public async ValueTask<DailyWordDTO> Handle(GetDailyWordQuery request, CancellationToken cancellationToken)
    {
        var dailyWord = request.Date.HasValue
            ? await dailyWordRepository.GetByDateAsync(request.Date.Value.Date, cancellationToken)
            : await dailyWordRepository.GetTodayAsync(cancellationToken);

        if (dailyWord is null)
            throw new KeyNotFoundException("Daily word not found for the requested date");

        bool hasPlayed = false;
        if (request.UserId.HasValue)
        {
            hasPlayed = await gameResultRepository.HasUserPlayedTodayAsync(request.UserId.Value, cancellationToken);
        }

        return new DailyWordDTO()
        {
            Id = dailyWord.Id,
            Date = dailyWord.Date,
            WordHash = dailyWord.WordHash,
            WordLength = dailyWord.Word.Length,
            TotalAttempts = dailyWord.TotalAttempts,
            TotalWins = dailyWord.TotalWins,
            TotalLosses = dailyWord.TotalLosses,
            WinRate = dailyWord.WinRate,
            HasUserPlayed = hasPlayed
        };
    }
}