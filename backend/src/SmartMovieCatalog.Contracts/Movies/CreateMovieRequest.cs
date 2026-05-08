namespace SmartMovieCatalog.Contracts.Movies;

public sealed record CreateMovieRequest
{
    public string? Title { get; init; }

    public string? OriginalTitle { get; init; }

    public int? ReleaseYear { get; init; }

    public string? CountryCode { get; init; }

    public string? OriginalLanguage { get; init; }

    public IReadOnlyCollection<string>? Genres { get; init; }

    public string? Director { get; init; }

    public string? Synopsis { get; init; }

    public int? DurationMinutes { get; init; }

    public string? AgeRating { get; init; }

    public int? ExternalId { get; init; }

    public string? Image { get; init; }
}
