namespace SmartMovieCatalog.Application.Features.Movies;

public sealed record CreateMovieCommand(
    string Title,
    string? OriginalTitle,
    int ReleaseYear,
    string CountryCode,
    string OriginalLanguage,
    IReadOnlyCollection<string>? Genres,
    string? Director,
    string? Synopsis,
    int? DurationMinutes,
    string? AgeRating);
