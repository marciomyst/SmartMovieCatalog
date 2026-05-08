namespace SmartMovieCatalog.Application.Features.Movies;

public sealed record PagedMovieSummaries(
    IReadOnlyCollection<MovieSummary> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages,
    bool HasPreviousPage,
    bool HasNextPage);
