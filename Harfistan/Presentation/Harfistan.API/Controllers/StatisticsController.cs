using Harfistan.Application.Features.Statistics.Queries;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Harfistan.API.Controllers;

/// <summary>
/// User statistics
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class StatisticsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get user statistics
    /// </summary>
    /// <remarks>
    /// Retrieve comprehensive statistics for a user.
    ///
    /// Sample request:
    ///
    ///     GET /api/statistics/123e4567-e89b-12d3-a456-426614174000
    ///
    /// Sample response:
    ///
    ///     {
    ///        "userId": "123e4567-e89b-12d3-a456-426614174000",
    ///        "displayName": "Player_1234",
    ///        "gamesPlayed": 42,
    ///        "gamesWon": 35,
    ///        "gamesLost": 7,
    ///        "winRate": 83.33,
    ///        "currentStreak": 5,
    ///        "maxStreak": 12,
    ///        "guessDistribution": [3, 8, 12, 7, 4, 1],
    ///        "averageAttempts": 3.4,
    ///        "averagePlayTimeSeconds": 95,
    ///        "totalPlayTimeSeconds": 3990,
    ///        "lastPlayedDate": "2025-02-10",
    ///        "memberSince": "2025-01-01"
    ///     }
    ///
    /// </remarks>
    /// <param name="userId">User ID</param>
    /// <returns>User statistics</returns>
    /// <response code="200">Returns user statistics</response>
    /// <response code="404">If user not found</response>
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserStatistics(Guid userId)
    {
        var query = new GetUserStatisticsQuery(userId);
        return Ok(await mediator.Send(query));
    }
}