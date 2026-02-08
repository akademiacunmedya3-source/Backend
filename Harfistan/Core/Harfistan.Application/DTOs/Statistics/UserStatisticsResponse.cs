namespace Harfistan.Application.DTOs.Statistics;

public class UserStatisticsResponse
{
    public Guid UserId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public int GamesPlayed { get; set; }
    public int GamesWon { get; set; }
    public int GamesLost { get; set; }
    public double WinRate { get; set; }
    public int CurrentStreak { get; set; }
    public int MaxStreak { get; set; }
    public List<int> GuessDistribution { get; set; } = new();
    public double AverageAttempts { get; set; }
    public int AveragePlayTimeSeconds { get; set; }
    public int TotalPlayTimeSeconds { get; set; }
    public DateTime? LastPlayedDate { get; set; }
    public DateTime MemberSince { get; set; }
}