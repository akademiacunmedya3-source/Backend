using Harfistan.Application.Abstractions.Repositories;
using Harfistan.Application.DTOs.LeaderBoards;
using Harfistan.Domain.Entities;
using Mediator;

namespace Harfistan.Application.Features.LeaderBoards.Queries.GetLeaderBoard;


public record GetLeaderBoardQuery(Guid? CurrentUserId, LeaderboardType LeaderboardType = LeaderboardType.WinRate, int Top = 100) : IRequest<LeaderBoardResponse>;

public class GetLeaderBoardQueryHandler(IUserRepository userRepository) : IRequestHandler<GetLeaderBoardQuery, LeaderBoardResponse>
{
    public async ValueTask<LeaderBoardResponse> Handle(GetLeaderBoardQuery request, CancellationToken cancellationToken)
    {
        var topPlayers = await userRepository.GetTopPlayersAsync(
            request.Top,
            request.LeaderboardType,
            cancellationToken);

        var entries = topPlayers
            .Select((user, index) => new LeaderboardEntry()
            {
                Rank = index + 1,
                UserId = user.Id,
                DisplayName = user.DisplayName ?? "Player",
                AvatarUrl = user.AvatarUrl,
                GamesWon = user.Stats?.GamesWon ?? 0,
                GamesPlayed = user.Stats?.GamesPlayed ?? 0,
                WinRate = user.Stats?.WinRate ?? 0,
                CurrentStreak = user.Stats?.CurrentStreak ?? 0,
                MaxStreak = user.Stats?.MaxStreak ?? 0,
                Score = CalculateScore(user.Stats, request.LeaderboardType),
                IsCurrentUser = user.Id == request.CurrentUserId
            }).ToList();
        
        LeaderboardEntry? currentUserEntry = null;
        if (request.CurrentUserId.HasValue){}
        {
            currentUserEntry = entries.FirstOrDefault(x=> x.IsCurrentUser);

            if (currentUserEntry == null)
            {
                var currentUser = await userRepository.GetByIdAsync(request.CurrentUserId.Value, cancellationToken);

                if (currentUser?.Stats != null)
                {
                    var rank = await userRepository.GetUserRankAsync(request.CurrentUserId.Value,request.LeaderboardType, cancellationToken);
                    currentUserEntry = new LeaderboardEntry()
                    {
                        Rank = rank,
                        UserId = currentUser.Id,
                        DisplayName = currentUser.DisplayName ?? "Player",
                        AvatarUrl = currentUser.AvatarUrl,
                        GamesWon = currentUser.Stats.GamesWon,
                        GamesPlayed = currentUser.Stats.GamesPlayed,
                        WinRate = currentUser.Stats.WinRate,
                        CurrentStreak = currentUser.Stats.CurrentStreak,
                        MaxStreak = currentUser.Stats.MaxStreak,
                        Score = CalculateScore(currentUser.Stats, request.LeaderboardType),
                        IsCurrentUser = true
                    };
                }
            }
        }
        var totalPlayers = await userRepository.GetTotalPlayersCountAsync(cancellationToken);

        return new LeaderBoardResponse()
        {
            Entries = entries,
            CurrentUser = currentUserEntry,
            TotalPlayers = totalPlayers
        };
    }
    
    private static double CalculateScore(
        UserStat stats,
        LeaderboardType type)
    {
        if (stats == null) return 0;

        return type switch
        {
            LeaderboardType.WinRate => stats.WinRate,
            LeaderboardType.TotalWins => stats.GamesWon,
            LeaderboardType.CurrentStreak => stats.CurrentStreak,
            LeaderboardType.MaxStreak => stats.MaxStreak,
            _ => 0
        };
    }
}