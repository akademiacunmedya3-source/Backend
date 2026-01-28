namespace Harfistan.Domain.Entities;

public sealed class DailyWord
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