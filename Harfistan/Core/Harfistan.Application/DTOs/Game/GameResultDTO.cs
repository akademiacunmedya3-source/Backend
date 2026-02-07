namespace Harfistan.Application.DTOs.Game;

public class GameResultDTO
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int GameResultId { get; set; }
    public UserStatsDto UpdatedStats { get; set; } = new();
    public string? CorrectWord { get; set; }
}

public class UserStatsDto
{
    public int GamesPlayed { get; set; }
    public int GamesWon { get; set; }
    public int GamesLost { get; set; }
    public double WinRate { get; set; }
    public int CurrentStreak { get; set; }
    public int MaxStreak { get; set; }
    public List<int> GuessDistribution { get; set; } = new();
    public double AverageAttempts { get; set; }
}