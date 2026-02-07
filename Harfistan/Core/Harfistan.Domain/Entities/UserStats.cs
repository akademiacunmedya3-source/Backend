namespace Harfistan.Domain.Entities;

public sealed class UserStat
{
    public Guid UserId { get; set; }
    public int GamesPlayed { get; set; }
    public int GamesWon { get; set; }
    public int GamesLost { get; set; }
    public int CurrentStreak { get; set; }
    public int MaxStreak { get; set; }
    public DateTime? LastPlayedDate { get; set; }
    public string GuessDistribution { get; set; } = "[0,0,0,0,0,0]";
    public double WinRate { get; set; }
    public double AverageAttempts { get; set; }
    public int TotalPlayTimeSeconds { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public User User { get; set; } = null!;
}