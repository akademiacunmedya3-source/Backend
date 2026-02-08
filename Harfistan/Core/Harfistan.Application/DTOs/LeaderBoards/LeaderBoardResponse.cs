namespace Harfistan.Application.DTOs.LeaderBoards;

public class LeaderBoardResponse
{
    public List<LeaderboardEntry> Entries { get; set; } = new();
    public LeaderboardEntry? CurrentUser { get; set; }
    public int TotalPlayers { get; set; }
}