namespace Harfistan.Domain.Entities;

public sealed class GameResult
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