namespace Harfistan.Application.Features.LeaderBoards;

public class LeaderboardEntry
{
    public int Rank { get; set; }
    public Guid UserId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public int GamesWon { get; set; }
    public int GamesPlayed { get; set; }
    public double WinRate { get; set; }
    public int CurrentStreak { get; set; }
    public int MaxStreak { get; set; }
    public double Score { get; set; }
    public bool IsCurrentUser { get; set; }
}