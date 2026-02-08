namespace Harfistan.Application.DTOs.Game;

public class GameHistoryResponse
{
    public List<GameHistoryEntry> Games { get; set; } = new();
    public int TotalGames { get; set; }
}

public class GameHistoryEntry
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Word { get; set; } = string.Empty;
    public bool IsWin { get; set; }
    public int Attempts { get; set; }
    public int DurationSeconds { get; set; }
    public List<string> Guesses { get; set; } = new();
    public bool IsHardMode { get; set; }
}