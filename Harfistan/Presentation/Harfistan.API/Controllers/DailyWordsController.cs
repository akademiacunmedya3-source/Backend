using Harfistan.Application.Features.DailyWords.Commands.CreateDailyWord;
using Harfistan.Application.Features.DailyWords.Queries;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Harfistan.API.Controllers;

/// <summary>
/// Daily word management
/// </summary>
[ApiController]
[Route("api/daily-words")]
[Produces("application/json")]
public class DailyWordsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get today's daily word
    /// </summary>
    /// <remarks>
    /// Returns today's word hash (not the actual word) and global statistics.
    /// 
    /// Sample request:
    /// 
    ///     GET /api/daily-words/today?userId=123e4567-e89b-12d3-a456-426614174000
    /// 
    /// </remarks>
    /// <param name="userId">Optional: User ID to check if already played</param>
    /// <returns>Daily word information</returns>
    /// <response code="200">Returns today's daily word</response>
    /// <response code="404">If today's word hasn't been created yet</response>
    [HttpGet("today")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTodayDailyWord([FromQuery] Guid? userId = null)
    {
        var query = new GetDailyWordQuery(userId, DateTime.Today);
        return Ok(await mediator.Send(query));
    }
    /// <summary>
    /// Get daily word by date
    /// </summary>
    /// <remarks>
    /// Retrieve word for a specific date (past or future).
    /// 
    /// Sample request:
    /// 
    ///     GET /api/daily-words/2025-02-10?userId=123e4567-e89b-12d3-a456-426614174000
    /// 
    /// </remarks>
    /// <param name="date">Date in YYYY-MM-DD format</param>
    /// <param name="userId">Optional: User ID to check if already played</param>
    /// <returns>Daily word information</returns>
    /// <response code="200">Returns the daily word</response>
    /// <response code="404">If word not found for date</response>
    [HttpGet("{date:datetime}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDailyWordByDate(DateTime date, [FromQuery] Guid? userId = null)
    {
        var query = new GetDailyWordQuery(userId, date);
        return Ok(await mediator.Send(query));
    }

    /// <summary>
    /// Create daily word (Admin)
    /// </summary>
    /// <remarks>
    /// Manually create a daily word for a specific date.
    /// If a wordId not provided, a random word will be selected.
    /// 
    /// Sample request:
    /// 
    ///     POST /api/daily-words
    ///     {
    ///        "date": "2025-02-15",
    ///        "wordLength": 5
    ///     }
    /// 
    /// </remarks>
    /// <returns>Created daily word ID</returns>
    /// <response code="201">Daily word created successfully</response>
    /// <response code="400">If date is in the past or word length invalid</response>
    /// <response code="409">If daily word already exists for date</response>
    [HttpPost]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateDailyWord([FromBody] CreateDailyWordCommand createDailyWordCommand)
    {
        var dailyWordId = await mediator.Send(createDailyWordCommand);
        return CreatedAtAction(nameof(GetDailyWordByDate), new { date = createDailyWordCommand.Date.Date },
            new { id = dailyWordId });
    }
}