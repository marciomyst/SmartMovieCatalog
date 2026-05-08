using Microsoft.AspNetCore.Mvc;
using SmartMovieCatalog.Api.Common;
using SmartMovieCatalog.Application.Features.Movies;
using SmartMovieCatalog.Contracts.Movies;
using Wolverine;

namespace SmartMovieCatalog.Api.Features.Movies.ListMovies;

public static class ListMoviesEndpoint
{
    public static void MapListMovies(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/movies", HandleAsync)
            .WithName("ListMovies")
            .WithTags("Movies")
            .AllowAnonymous()
            .AddEndpointFilter<ValidationFilter<ListMoviesRequest>>()
            .Produces<PagedMovieSummaryResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> HandleAsync(
        [AsParameters] ListMoviesRequest request,
        IMessageBus messageBus,
        CancellationToken cancellationToken)
    {
        PagedMovieSummaries movies = await messageBus.InvokeAsync<PagedMovieSummaries>(
            new ListMoviesQuery(
                NormalizeQuery(request.Query),
                request.Page,
                request.PageSize),
            cancellationToken);

        MovieSummaryResponse[] items = movies.Items
            .Select(movie => new MovieSummaryResponse(
                movie.Id.ToString(),
                movie.Title,
                movie.ReleaseYear,
                movie.CountryCode,
                movie.Genres,
                movie.Director,
                movie.PosterUrl,
                movie.CreatedAt))
            .ToArray();

        return Results.Ok(new PagedMovieSummaryResponse(
            items,
            movies.Page,
            movies.PageSize,
            movies.TotalCount,
            movies.TotalPages,
            movies.HasPreviousPage,
            movies.HasNextPage));
    }

    private static string? NormalizeQuery(string? query)
    {
        string? trimmedQuery = query?.Trim();

        return string.IsNullOrEmpty(trimmedQuery) ? null : trimmedQuery;
    }
}
