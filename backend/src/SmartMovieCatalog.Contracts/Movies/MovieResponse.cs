namespace SmartMovieCatalog.Contracts.Movies;

public sealed record MovieResponse(
    string Id,
    string Title,
    string? OriginalTitle,
    int ReleaseYear,
    string CountryCode,
    string OriginalLanguage,
    IReadOnlyCollection<string> Genres,
    string? Director,
    string? Synopsis,
    int? DurationMinutes,
    string? AgeRating,
    int? ExternalId,
    string? Image);
