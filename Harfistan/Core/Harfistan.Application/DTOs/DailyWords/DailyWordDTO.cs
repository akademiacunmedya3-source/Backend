namespace Harfistan.Application.DTOs.DailyWords;

public class DailyWordDTO
{
    public int Id { get; init; }
    public DateTime Date { get; init; }
    public string WordHash { get; init; } = string.Empty;
    public int WordLength { get; init; }
    public int TotalAttempts { get; init; }
    public int TotalWins { get; init; }
    public int TotalLosses { get; init; }
    public double WinRate { get; init; }
    public bool HasUserPlayed { get; init; }
}