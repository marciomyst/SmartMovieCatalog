namespace SmartMovieCatalog.Application.Features.Movies;

public sealed record ListMoviesQuery(
    string? Query,
    int Page,
    int PageSize);
