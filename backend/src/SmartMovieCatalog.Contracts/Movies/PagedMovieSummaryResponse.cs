namespace SmartMovieCatalog.Contracts.Movies;

public sealed record PagedMovieSummaryResponse(
    IReadOnlyCollection<MovieSummaryResponse> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages,
    bool HasPreviousPage,
    bool HasNextPage);
