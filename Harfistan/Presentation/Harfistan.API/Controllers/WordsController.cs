using Harfistan.Application.Features.Words.Queries.ValidateWord;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Harfistan.API.Controllers;

/// <summary>
/// Word validation
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class WordsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Validate word (GET)
    /// </summary>
    /// <remarks>
    /// Check if a word exists in the Turkish dictionary.
    /// Case-insensitive, automatically normalized.
    /// 
    /// Sample request:
    /// 
    ///     GET /api/words/validate/KEMAN
    /// 
    /// </remarks>
    /// <param name="word">Word to validate</param>
    /// <returns>Validation result</returns>
    /// <response code="200">Returns validation result</response>
    /// <response code="400">If a word format is invalid</response>
    [HttpGet("validate/{word}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ValidateWord(string word)
    {
        var query = new ValidateWordQuery(word);
        return Ok(await mediator.Send(query));
    }

    /// <summary>
    /// Validate word (POST)
    /// </summary>
    /// <remarks>
    /// Alternative endpoint for word validation via POST.
    /// Useful for special characters or longer words.
    /// 
    /// Sample request:
    /// 
    ///     POST /api/words/validate
    ///     {
    ///        "word": "KEMAN"
    ///     }
    /// 
    /// </remarks>
    /// <returns>Validation result</returns>
    /// <response code="200">Returns validation result</response>
    /// <response code="400">If a word format is invalid</response>
    [HttpPost("validate")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ValidateWordPost([FromBody] ValidateWordQuery wordQuery)
    {
        var result = await mediator.Send(wordQuery);
        return Ok(result);
    }
}