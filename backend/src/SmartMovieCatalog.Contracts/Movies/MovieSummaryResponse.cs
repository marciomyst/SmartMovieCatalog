namespace SmartMovieCatalog.Contracts.Movies;

public sealed record MovieSummaryResponse(
    string Id,
    string Title,
    int ReleaseYear,
    string CountryCode,
    IReadOnlyCollection<string> Genres,
    string? Director,
    string? PosterUrl,
    DateTimeOffset CreatedAt);
