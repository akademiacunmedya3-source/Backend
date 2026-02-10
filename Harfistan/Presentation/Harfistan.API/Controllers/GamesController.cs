using Harfistan.Application.Features.Games.Commands;
using Harfistan.Application.Features.Games.Queries.GetGameHistory;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Harfistan.API.Controllers;

/// <summary>
/// Game operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class GamesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Submit game result
    /// </summary>
    /// <remarks>
    /// Submit the result of a completed game.
    /// Can only be submitted once per user per day.
    ///
    /// Sample request:
    ///
    ///     POST /api/game/submit
    ///     {
    ///        "userId": "123e4567-e89b-12d3-a456-426614174000",
    ///        "isWin": true,
    ///        "attempts": 4,
    ///        "durationSeconds": 120,
    ///        "guesses": ["KEMAN", "KELAM", "KEMAL", "KALEM"],
    ///        "isHardMode": false,
    ///        "deviceType": "Android"
    ///     }
    ///
    /// </remarks>
    /// <param name="command">Game result data</param>
    /// <returns>Updated user statistics</returns>
    /// <response code="200">Game result submitted successfully</response>
    /// <response code="400">If data is invalid</response>
    /// <response code="404">If user or daily word not found</response>
    /// <response code="409">If user already played today</response>
    [HttpPost("submit")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SubmitGameResult([FromBody] SubmitGameResultCommand submitGameResultCommand)
    {
        var result = mediator.Send(submitGameResultCommand);
        return Ok(result);
    }

    /// <summary>
    /// Get user game history
    /// </summary>
    /// <remarks>
    /// Retrieve paginated game history for a user.
    ///
    /// Sample request:
    ///
    ///     GET /api/game/history/123e4567-e89b-12d3-a456-426614174000?pageNumber=1&amp;pageSize=20
    ///
    /// </remarks>
    /// <param name="userId">User ID</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 20, max: 50)</param>
    /// <returns>Paginated game history</returns>
    /// <response code="200">Returns game history</response>
    /// <response code="404">If user not found</response>
    [HttpGet("history/{userId:guid}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGameHistory(Guid userId, [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20)
    {
        pageSize = Math.Min(pageSize, 50);
        var query = new GetGameHistoryQuery(userId, pageNumber, pageSize);
        return Ok(await mediator.Send(query));
    }
    
    
}