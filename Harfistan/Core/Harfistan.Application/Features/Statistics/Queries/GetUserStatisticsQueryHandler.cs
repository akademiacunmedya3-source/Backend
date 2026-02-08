using System.Text.Json;
using Harfistan.Application.Abstractions.Repositories;
using Harfistan.Application.DTOs.Statistics;
using Harfistan.Application.Exceptions;
using Mediator;

namespace Harfistan.Application.Features.Statistics.Queries;

public record GetUserStatisticsQuery(Guid UserId) : IRequest<UserStatisticsResponse>;

public class GetUserStatisticsQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetUserStatisticsQuery, UserStatisticsResponse>
{
    public async ValueTask<UserStatisticsResponse> Handle(GetUserStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken) ??
                   throw new NotFoundException("User", request.UserId);
        if (user.Stats is null)
            throw new NotFoundException($"Statistics not found for user{request.UserId}");

        var guessDistribution = JsonSerializer.Deserialize<List<int>>(user.Stats.GuessDistribution) ??
                                new List<int> { 0, 0, 0, 0, 0, 0 };

        var avgPlayTimeSeconds = user.Stats.GamesPlayed > 0
            ? user.Stats.TotalPlayTimeSeconds / user.Stats.GamesPlayed
            : 0;

        return new UserStatisticsResponse()
        {
            UserId = user.Id,
            DisplayName = user.DisplayName,
            AvatarUrl = user.AvatarUrl,
            GamesPlayed = user.Stats.GamesPlayed,
            GamesWon = user.Stats.GamesWon,
            GamesLost = user.Stats.GamesLost,
            WinRate = user.Stats.WinRate,
            CurrentStreak = user.Stats.CurrentStreak,
            MaxStreak = user.Stats.MaxStreak,
            GuessDistribution = guessDistribution,
            AverageAttempts = user.Stats.AverageAttempts,
            AveragePlayTimeSeconds = avgPlayTimeSeconds,
            TotalPlayTimeSeconds = user.Stats.TotalPlayTimeSeconds,
            LastPlayedDate = user.Stats.LastPlayedDate,
            MemberSince = user.CreatedAt
        };
    }
}