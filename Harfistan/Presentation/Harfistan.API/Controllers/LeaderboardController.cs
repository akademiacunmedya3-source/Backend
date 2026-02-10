using Harfistan.Application.Features.LeaderBoards.Queries.GetLeaderBoard;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Harfistan.API.Controllers;

/// <summary>
/// Leaderboard
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class LeaderboardController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get leaderboard
    /// </summary>
    /// <remarks>
    /// Retrieve top players sorted by different criteria.
    ///
    /// Sample requests:
    ///
    ///     GET /api/leaderboard?type=WinRate&amp;top=100
    ///     GET /api/leaderboard?type=TotalWins&amp;userId=123e4567-e89b-12d3-a456-426614174000
    ///     GET /api/leaderboard?type=CurrentStreak&amp;top=50
    ///
    /// Leaderboard Types:
    /// - WinRate: Sort by win percentage (min 5 games required)
    /// - TotalWins: Sort by total games won
    /// - CurrentStreak: Sort by active streak
    /// - MaxStreak: Sort by best streak ever
    ///
    /// </remarks>
    /// <param name="type">Sorting criteria (default: WinRate)</param>
    /// <param name="top">Number of players to return (default: 100, max: 100)</param>
    /// <param name="userId">Optional: Current user ID to show their rank</param>
    /// <returns>Leaderboard with top players</returns>
    /// <response code="200">Returns leaderboard</response>
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLeaderBoard([FromQuery] LeaderboardType type = LeaderboardType.WinRate,
        [FromQuery] int top = 100,
        [FromQuery] Guid? userId = null)
    {
        top = Math.Min(top, 100);
        var query = new GetLeaderBoardQuery(userId, type, top);
        return Ok(await mediator.Send(query));
    }

    /// <summary>
    /// Get user rank
    /// </summary>
    /// <remarks>
    /// Get specific user's rank in the leaderboard.
    ///
    /// Sample request:
    ///
    ///     GET /api/leaderboard/rank/123e4567-e89b-12d3-a456-426614174000?type=WinRate
    ///
    /// </remarks>
    /// <param name="userId">User ID</param>
    /// <param name="type">Leaderboard type (default: WinRate)</param>
    /// <returns>User's rank information</returns>
    /// <response code="200">Returns user rank</response>
    /// <response code="404">If user not found</response>
    [HttpGet("rank/{userId:guid}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserRank(Guid userId,
        [FromQuery] LeaderboardType leaderboardType = LeaderboardType.WinRate)
    {
        var query = new GetLeaderBoardQuery(userId, leaderboardType, 1);
        var result = await mediator.Send(query);

        if (result.CurrentUser is null)
            return NotFound(new { message = "User not found or hasn't played yet" });

        return Ok(new
        {
            userId,
            rank = result.CurrentUser.Rank,
            totalPlayers = result.TotalPlayers,
            type = leaderboardType.ToString(),
            score = result.CurrentUser.Score,
            displayName = result.CurrentUser.DisplayName
        });
    }
}