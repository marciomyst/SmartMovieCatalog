using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMovieCatalog.Api.Common;
using SmartMovieCatalog.Application.Features.Auth;
using SmartMovieCatalog.Contracts.Auth;

namespace SmartMovieCatalog.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly AuthenticateUser _authenticateUser;
    private readonly GetCurrentUser _getCurrentUser;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        AuthenticateUser authenticateUser,
        GetCurrentUser getCurrentUser,
        ILogger<AuthController> logger)
    {
        _authenticateUser = authenticateUser;
        _getCurrentUser = getCurrentUser;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    [ProducesResponseType<AuthenticateResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthenticateResponse>> Authenticate(
        [FromBody] AuthenticateRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        AuthenticationResult result = await _authenticateUser.AuthenticateAsync(
            request.Email,
            request.Password,
            cancellationToken);

        if (!result.Succeeded)
        {
            _logger.LogInformation("Authentication failed.");
            return Unauthorized(AuthProblemDetails.AuthenticationFailed(HttpContext));
        }

        return Ok(new AuthenticateResponse(
            result.UserId,
            result.Email!,
            result.AccessToken!,
            result.AccessTokenExpiresAtUtc!.Value));
    }

    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType<CurrentUserResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CurrentUserResponse>> Me(CancellationToken cancellationToken)
    {
        CurrentUserResult result = await _getCurrentUser.GetAsync(cancellationToken);

        if (!result.Succeeded)
        {
            return Unauthorized(AuthProblemDetails.CurrentUserUnavailable(HttpContext));
        }

        return Ok(new CurrentUserResponse(
            result.UserId,
            result.Email!,
            result.Name!,
            result.Roles,
            result.MustChangePasswordOnFirstLogin));
    }
}
