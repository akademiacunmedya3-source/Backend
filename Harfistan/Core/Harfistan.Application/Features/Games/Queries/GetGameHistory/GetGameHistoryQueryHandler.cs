using System.Text.Json;
using Harfistan.Application.Abstractions.Repositories;
using Harfistan.Application.Commons;
using Harfistan.Application.DTOs.Game;
using Harfistan.Application.Exceptions;
using Mediator;

namespace Harfistan.Application.Features.Games.Queries.GetGameHistory;

public record GetGameHistoryQuery(Guid UserId, int PageNumber = 1, int PageSize = 20)
    : IRequest<PagedResult<GameHistoryEntry>>;

public class GetGameHistoryQueryHandler(IUserRepository userRepository, IGameResultRepository gameResultRepository)
    : IRequestHandler<GetGameHistoryQuery, PagedResult<GameHistoryEntry>>
{
    public async ValueTask<PagedResult<GameHistoryEntry>> Handle(GetGameHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var userExists = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (userExists is null)
            throw new NotFoundException("User", request.UserId);

        var (games, totalCount) = await gameResultRepository.GetUserHistoryPagedAsync(request.UserId,
            request.PageNumber, request.PageSize, cancellationToken);

        var entries = games.Select(game =>
        {
            var guesses = JsonSerializer.Deserialize<List<string>>(game.GuessesJson) ?? new List<string>();

            return new GameHistoryEntry()
            {
                Id = game.Id,
                Date = game.DailyWord.Date,
                Word = game.DailyWord.Word.Text,
                IsWin = game.IsWin,
                Attempts = game.Attempts,
                DurationSeconds = game.DurationSeconds,
                Guesses = guesses,
                IsHardMode = game.IsHardMode
            };
        }).ToList();

        return new PagedResult<GameHistoryEntry>
        {
            Items = entries,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}