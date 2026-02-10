using Harfistan.Application.Features.Auths.Commands.CreateAnonymousUser;
using Harfistan.Application.Features.Auths.Commands.LoginWithGoogle;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace Harfistan.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Create or retrieve anonymous user
    /// </summary>
    /// <remarks>
    /// Creates a new anonymous user or retrieves existing one by device ID.
    /// 
    /// Sample request:
    /// 
    ///     POST /api/auth/anonymous
    ///     {
    ///        "deviceId": "abc123-device-id",
    ///        "deviceType": "Android",
    ///        "deviceModel": "Samsung Galaxy S23"
    ///     }
    /// 
    /// </remarks>
    /// <param name="command">Device information</param>
    /// <returns>User details with isNewUser flag</returns>
    /// <response code="200">Returns the user details</response>
    /// <response code="400">If device ID is invalid</response>
    [HttpPost("anonymous")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAnonymousUser(
        [FromBody] CreateAnonymousUserCommand createAnonymousUserCommand)
    {
        var result = await mediator.Send(createAnonymousUserCommand);
        return Ok(result);
    }
    /// <summary>
    /// Login with Google OAuth
    /// </summary>
    /// <remarks>
    /// Authenticates user with Google OAuth credentials.
    /// Creates new account if user doesn't exist.
    /// 
    /// Sample request:
    /// 
    ///     POST /api/auth/google
    ///     {
    ///        "googleId": "1234567890",
    ///        "email": "user@gmail.com",
    ///        "displayName": "John Doe",
    ///        "avatarUrl": "https://...",
    ///        "deviceType": "Android"
    ///     }
    /// 
    /// </remarks>
    /// <param name="command">Google OAuth data</param>
    /// <returns>User details with isNewUser flag</returns>
    /// <response code="200">Returns the user details</response>
    /// <response code="400">If Google credentials are invalid</response>
    [HttpGet("google")]
    [ProducesResponseType(typeof(object),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginWithGoogle([FromBody] LoginWithGoogleCommand loginWithGoogleCommand)
    {
        var result = await mediator.Send(loginWithGoogleCommand);
        return Ok(result);
    }
    
    /// <summary>
    /// Health check
    /// </summary>
    /// <returns>Service health status</returns>
    /// <response code="200">Service is healthy</response>
    [HttpGet("health")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public IActionResult Health()
    {
        return Ok(new
        {
            status = "healthy",
            service = "Auth API",
            timestamp = DateTime.UtcNow
        });
    }
}
