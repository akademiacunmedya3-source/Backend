using System.Text.Json;
using Harfistan.Application.DTOs.Game;
using Harfistan.Domain.Entities;
using Mediator;

namespace Harfistan.Application.Features.Games.Commands;

public record SubmitGameResultCommand : IRequest<GameResultDTO>
{
    public Guid UserId { get; set; }
    public bool IsWin { get; set; }
    public int Attempts { get; set; }
    public int DurationSeconds { get; set; }
    public List<string> Guesses { get; set; } = new();
    public bool IsHardMode { get; set; }
    public string? DeviceType { get; set; }
}

public class SubmitGameResultCommandHandler
{
    
    
    
    private static UserStatsDto MapToStatsDto(UserStat stat)
    {
        var distribution = JsonSerializer.Deserialize<List<int>>(stat.GuessDistribution) ??
                           new List<int> { 0, 0, 0, 0, 0, 0 };

        return new UserStatsDto()
        {
            GamesPlayed = stat.GamesPlayed,
            GamesWon = stat.GamesWon,
            GamesLost = stat.GamesLost,
            WinRate = stat.WinRate,
            CurrentStreak = stat.CurrentStreak,
            MaxStreak = stat.MaxStreak,
            GuessDistribution = distribution,
            AverageAttempts = stat.AverageAttempts,
        };
    }
}