using System.Security.Cryptography;

namespace Harfistan.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? GoogleId { get; set; }
    public string? DeviceId { get; set; }
    public string AuthProvider { get; set; } = "Anonymous";
    public string? DisplayName { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public UserStats? Stats { get; set; }
    public List<UserSession> Sessions { get; set; } = new();
    public List<GameResult> GameResults { get; set; } = new();
}

public class UserStats
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
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
}

public class UserSession
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? DeviceType { get; set; }
    public string? DeviceModel { get; set; }
    public DateTime LoginAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastActivityAt { get; set; }
    public DateTime? LogoutAt { get; set; }
    public bool IsActive { get; set; } = true;
}

public class GameResult
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public int DailyWordId { get; set; }
    public DailyWord DailyWord { get; set; } = null!;
    public bool IsWin { get; set; }
    public int Attempts { get; set; }
    public int DurationSeconds { get; set; }
    public string GuessesJson { get; set; } = "[]";
    public bool IsHardMode { get; set; }
    public string? DeviceType { get; set; }
    public DateTime PlayedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
}
public class Word
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public int Length { get; set; }
    public bool IsCommon { get; set; } = true;
    public string? Category { get; set; }
    public int DifficultyLevel { get; set; } = 1;
    public int TimesUsed { get; set; }
    public double AverageSuccessRate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastUsedAt { get; set; }
    public List<DailyWord> DailyWords { get; set; } = new();
}
public class DailyWord
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int WordId { get; set; }
    public Word Word { get; set; } = null!;
    public string WordHash { get; set; } = string.Empty;
    public int TotalAttempts { get; set; }
    public int TotalWins { get; set; }
    public int TotalLosses { get; set; }
    public int TotalPlayers { get; set; }
    public double WinRate { get; set; }
    public double AverageAttempts { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<GameResult> GameResults { get; set; } = new();
}