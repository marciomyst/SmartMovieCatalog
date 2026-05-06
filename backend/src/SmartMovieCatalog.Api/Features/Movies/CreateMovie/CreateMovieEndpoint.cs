using Microsoft.AspNetCore.Mvc;
using SmartMovieCatalog.Api.Common;
using SmartMovieCatalog.Application.Features.Movies;
using SmartMovieCatalog.Contracts.Movies;
using Wolverine;

namespace SmartMovieCatalog.Api.Features.Movies.CreateMovie;

public static class CreateMovieEndpoint
{
    public static void MapCreateMovie(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/movies", HandleAsync)
            .WithName("CreateMovie")
            .WithTags("Movies")
            .AllowAnonymous()
            .AddEndpointFilter<ValidationFilter<CreateMovieRequest>>()
            .Produces<MovieResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] CreateMovieRequest? request,
        IMessageBus messageBus,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        CreatedMovie createdMovie = await messageBus.InvokeAsync<CreatedMovie>(
            new CreateMovieCommand(
                request.Title!,
                request.OriginalTitle,
                request.ReleaseYear!.Value,
                request.CountryCode!,
                request.OriginalLanguage!,
                request.Genres,
                request.Director,
                request.Synopsis,
                request.DurationMinutes,
                request.AgeRating),
            cancellationToken);

        MovieResponse response = new(
            createdMovie.Id.ToString(),
            createdMovie.Title,
            createdMovie.OriginalTitle,
            createdMovie.ReleaseYear,
            createdMovie.CountryCode,
            createdMovie.OriginalLanguage,
            createdMovie.Genres,
            createdMovie.Director,
            createdMovie.Synopsis,
            createdMovie.DurationMinutes,
            createdMovie.AgeRating);

        return Results.Created($"/api/movies/{response.Id}", response);
    }
}
