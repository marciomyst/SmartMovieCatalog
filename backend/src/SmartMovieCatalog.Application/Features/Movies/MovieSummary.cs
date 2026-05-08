namespace SmartMovieCatalog.Application.Features.Movies;

public sealed record MovieSummary(
    Guid Id,
    string Title,
    int ReleaseYear,
    string CountryCode,
    IReadOnlyCollection<string> Genres,
    string? Director,
    string? PosterUrl,
    DateTimeOffset CreatedAt);
