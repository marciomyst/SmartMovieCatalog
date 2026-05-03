using Microsoft.AspNetCore.Mvc;

namespace SmartMovieCatalog.Api.Common;

public static class AuthProblemDetails
{
    public static ProblemDetails AuthenticationFailed(HttpContext httpContext)
    {
        return CreateUnauthorized(
            httpContext,
            "Authentication failed.",
            "The supplied credentials could not be authenticated.",
            "https://smartmoviecatalog/problems/authentication-failed");
    }

    public static ProblemDetails Unauthorized(HttpContext httpContext)
    {
        return CreateUnauthorized(
            httpContext,
            "Unauthorized.",
            "A valid bearer token is required.",
            "https://smartmoviecatalog/problems/unauthorized");
    }

    public static ProblemDetails CurrentUserUnavailable(HttpContext httpContext)
    {
        return CreateUnauthorized(
            httpContext,
            "Unauthorized.",
            "The authenticated user context is no longer valid.",
            "https://smartmoviecatalog/problems/current-user-unavailable");
    }

    private static ProblemDetails CreateUnauthorized(HttpContext httpContext, string title, string detail, string type)
    {
        ProblemDetails problemDetails = new()
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = title,
            Detail = detail,
            Type = type,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
        return problemDetails;
    }
}
