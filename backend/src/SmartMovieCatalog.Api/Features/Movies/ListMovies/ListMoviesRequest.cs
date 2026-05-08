namespace SmartMovieCatalog.Api.Features.Movies.ListMovies;

public sealed record ListMoviesRequest
{
    public string? Query { get; init; }

    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 12;
}
