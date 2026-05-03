using Microsoft.AspNetCore.Mvc;
using SmartMovieCatalog.Api.Common;
using SmartMovieCatalog.Application;
using SmartMovieCatalog.Application.Features.Auth;
using SmartMovieCatalog.Contracts.Auth;
using Wolverine;

namespace SmartMovieCatalog.Api.Features.Auth.Authenticate;

public static class AuthenticateEndpoint
{
    public static void MapAuthenticate(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/authenticate", HandleAsync)
            .WithName("Authenticate")
            .WithTags("Auth")
            .AllowAnonymous()
            .AddEndpointFilter<ValidationFilter<AuthenticateRequest>>()
            .Produces<AuthenticateResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized, "application/problem+json");
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] AuthenticateRequest? request,
        HttpContext httpContext,
        IMessageBus messageBus,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        Result<AuthenticatedUser, AuthenticationFailure> result =
            await messageBus.InvokeAsync<Result<AuthenticatedUser, AuthenticationFailure>>(
                new AuthenticateUserCommand(request.Email, request.Password),
                cancellationToken);

        return result.Match(
            authenticatedUser => Results.Ok(new AuthenticateResponse(
                authenticatedUser.UserId,
                authenticatedUser.Email,
                authenticatedUser.AccessToken,
                authenticatedUser.AccessTokenExpiresAtUtc)),
            failure =>
            {
                ILogger logger = loggerFactory.CreateLogger("SmartMovieCatalog.Auth.Authenticate");
                logger.LogInformation("Authentication failed.");

                return Results.Json(
                    AuthProblemDetails.AuthenticationFailed(httpContext),
                    statusCode: StatusCodes.Status401Unauthorized,
                    contentType: "application/problem+json");
            });
    }
}
