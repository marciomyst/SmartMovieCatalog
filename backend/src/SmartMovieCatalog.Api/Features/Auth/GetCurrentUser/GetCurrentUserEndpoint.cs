using Microsoft.AspNetCore.Mvc;
using SmartMovieCatalog.Api.Common;
using SmartMovieCatalog.Application;
using SmartMovieCatalog.Application.Features.Auth;
using SmartMovieCatalog.Contracts.Auth;
using Wolverine;

namespace SmartMovieCatalog.Api.Features.Auth.GetCurrentUser;

public static class GetCurrentUserEndpoint
{
    public static void MapGetCurrentUser(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/auth/me", HandleAsync)
            .WithName("GetCurrentUser")
            .WithTags("Auth")
            .RequireAuthorization()
            .Produces<CurrentUserResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized, "application/problem+json");
    }

    private static async Task<IResult> HandleAsync(
        HttpContext httpContext,
        IMessageBus messageBus,
        CancellationToken cancellationToken)
    {
        Result<CurrentUserProfile, AuthenticationFailure> result =
            await messageBus.InvokeAsync<Result<CurrentUserProfile, AuthenticationFailure>>(
                new GetCurrentUserQuery(),
                cancellationToken);

        return result.Match<IResult>(
            currentUser => Results.Ok(new CurrentUserResponse(
                currentUser.UserId,
                currentUser.Email,
                currentUser.Name,
                currentUser.Roles,
                currentUser.MustChangePasswordOnFirstLogin)),
            failure => Results.Json(
                AuthProblemDetails.CurrentUserUnavailable(httpContext),
                statusCode: StatusCodes.Status401Unauthorized,
                contentType: "application/problem+json"));
    }
}
