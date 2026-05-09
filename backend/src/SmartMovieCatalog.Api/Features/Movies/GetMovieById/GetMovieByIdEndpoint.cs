using Microsoft.AspNetCore.Mvc;
using SmartMovieCatalog.Application;
using SmartMovieCatalog.Application.Features.Movies;
using SmartMovieCatalog.Contracts.Movies;
using Wolverine;

namespace SmartMovieCatalog.Api.Features.Movies.GetMovieById;

public static class GetMovieByIdEndpoint
{
    private const string ProblemJsonContentType = "application/problem+json";
    public static void MapGetMovieById(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/movies/{id}", HandleAsync)
            .WithName("GetMovieById")
            .WithTags("Movies")
            .AllowAnonymous()
            .Produces<MovieDetailsResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest, ProblemJsonContentType)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound, ProblemJsonContentType);
    }

    private static async Task<IResult> HandleAsync(
        string id,
        HttpContext httpContext,
        IMessageBus messageBus,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out Guid movieId))
        {
            return Results.Json(
                CreateInvalidMovieIdProblem(httpContext),
                statusCode: StatusCodes.Status400BadRequest,
                contentType: ProblemJsonContentType);
        }

        Result<MovieDetails, GetMovieByIdFailure> result =
            await messageBus.InvokeAsync<Result<MovieDetails, GetMovieByIdFailure>>(
                new GetMovieByIdQuery(movieId),
                cancellationToken);

        return result.Match<IResult>(
            details => Results.Ok(new MovieDetailsResponse(
                details.Id.ToString(),
                details.Title,
                details.OriginalTitle,
                details.ReleaseYear,
                details.CountryCode,
                details.OriginalLanguage,
                details.Genres,
                details.Director,
                details.Synopsis,
                details.DurationMinutes,
                details.AgeRating,
                details.ExternalId,
                details.PosterUrl,
                details.CreatedAt)),
            _ => Results.Json(
                CreateMovieNotFoundProblem(httpContext, id),
                statusCode: StatusCodes.Status404NotFound,
                contentType: ProblemJsonContentType));
    }

    private static ProblemDetails CreateInvalidMovieIdProblem(HttpContext httpContext)
    {
        ProblemDetails problemDetails = new()
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Invalid movie id.",
            Detail = "Movie id must be a valid GUID.",
            Type = "https://smartmoviecatalog/problems/invalid-movie-id",
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
        return problemDetails;
    }

    private static ProblemDetails CreateMovieNotFoundProblem(HttpContext httpContext, string id)
    {
        ProblemDetails problemDetails = new()
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Movie not found.",
            Detail = $"Movie '{id}' was not found.",
            Type = "https://smartmoviecatalog/problems/movie-not-found",
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
        return problemDetails;
    }
}
