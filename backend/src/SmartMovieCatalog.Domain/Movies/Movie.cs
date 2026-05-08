using SmartMovieCatalog.Domain.Common;

namespace SmartMovieCatalog.Domain.Movies;

public sealed class Movie : AggregateRoot
{
    private readonly List<MovieGenre> _genres = [];

    private Movie()
    {
        Title = "Placeholder";
        ReleaseYear = 1888;
        CountryCode = "US";
        OriginalLanguage = "en";
        CreatedAtUtc = DateTimeOffset.UnixEpoch;
    }

    private Movie(
        MovieId id,
        string title,
        string? originalTitle,
        int releaseYear,
        string countryCode,
        string originalLanguage,
        IEnumerable<Genre> genres,
        string? director,
        string? synopsis,
        int? durationMinutes,
        string? ageRating,
        int? externalId,
        string? image,
        DateTimeOffset createdAtUtc)
    {
        Id = id.Value;
        Title = NormalizeRequired(title, nameof(title));
        OriginalTitle = NormalizeOptional(originalTitle);
        ReleaseYear = ValidateReleaseYear(releaseYear);
        CountryCode = NormalizeCountryCode(countryCode);
        OriginalLanguage = NormalizeRequired(originalLanguage, nameof(originalLanguage));
        Director = NormalizeOptional(director);
        Synopsis = NormalizeOptional(synopsis);
        DurationMinutes = ValidateDuration(durationMinutes);
        AgeRating = NormalizeOptional(ageRating);
        ExternalId = ValidateExternalId(externalId);
        Image = NormalizeImage(image);
        CreatedAtUtc = createdAtUtc;
        ReplaceGenres(genres);
    }

    public string Title { get; private set; }

    public string? OriginalTitle { get; private set; }

    public int ReleaseYear { get; private set; }

    public string CountryCode { get; private set; }

    public string OriginalLanguage { get; private set; }

    public IReadOnlyCollection<MovieGenre> Genres => _genres.AsReadOnly();

    public string? Director { get; private set; }

    public string? Synopsis { get; private set; }

    public int? DurationMinutes { get; private set; }

    public string? AgeRating { get; private set; }

    public int? ExternalId { get; private set; }

    public string? Image { get; private set; }

    public DateTimeOffset CreatedAtUtc { get; private set; }

    public static Movie Create(
        MovieId id,
        string title,
        string? originalTitle,
        int releaseYear,
        string countryCode,
        string originalLanguage,
        IEnumerable<Genre>? genres,
        string? director,
        string? synopsis,
        int? durationMinutes,
        string? ageRating,
        int? externalId,
        string? image,
        DateTimeOffset createdAtUtc)
    {
        return new Movie(
            id,
            title,
            originalTitle,
            releaseYear,
            countryCode,
            originalLanguage,
            genres ?? [],
            director,
            synopsis,
            durationMinutes,
            ageRating,
            externalId,
            image,
            createdAtUtc);
    }

    private static string NormalizeRequired(string? value, string parameterName)
    {
        return Guard.AgainstNullOrWhiteSpace(value, parameterName).Trim();
    }

    private static string? NormalizeOptional(string? value)
    {
        string? trimmed = value?.Trim();

        return string.IsNullOrEmpty(trimmed) ? null : trimmed;
    }

    private static int ValidateReleaseYear(int releaseYear)
    {
        int maxReleaseYear = DateTimeOffset.UtcNow.Year + 1;
        if (releaseYear < 1888 || releaseYear > maxReleaseYear)
        {
            throw new ArgumentOutOfRangeException(
                nameof(releaseYear),
                releaseYear,
                $"Release year must be between 1888 and {maxReleaseYear}.");
        }

        return releaseYear;
    }

    private static string NormalizeCountryCode(string? countryCode)
    {
        string normalizedCountryCode = NormalizeRequired(countryCode, nameof(countryCode)).ToUpperInvariant();
        if (normalizedCountryCode.Length != 2 || !normalizedCountryCode.All(char.IsLetter))
        {
            throw new ArgumentException("Country code must contain exactly two letters.", nameof(countryCode));
        }

        return normalizedCountryCode;
    }

    private static int? ValidateDuration(int? durationMinutes)
    {
        if (durationMinutes <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(durationMinutes),
                durationMinutes,
                "Duration must be positive when supplied.");
        }

        return durationMinutes;
    }

    private static int? ValidateExternalId(int? externalId)
    {
        if (externalId <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(externalId),
                externalId,
                "External ID must be positive when supplied.");
        }

        return externalId;
    }

    private static string? NormalizeImage(string? image)
    {
        string? normalizedImage = NormalizeOptional(image);
        if (normalizedImage is null)
        {
            return null;
        }

        if (!IsRelativeImagePath(normalizedImage))
        {
            throw new ArgumentException("Image must be a relative path.", nameof(image));
        }

        return normalizedImage;
    }

    private static bool IsRelativeImagePath(string image)
    {
        return image.StartsWith("/", StringComparison.Ordinal) &&
            !image.StartsWith("//", StringComparison.Ordinal) &&
            !image.Contains('\\', StringComparison.Ordinal);
    }

    private void ReplaceGenres(IEnumerable<Genre> genres)
    {
        MovieGenre[] distinctGenres = genres
            .DistinctBy(genre => genre.NormalizedName)
            .Select(genre => MovieGenre.Create(Id, genre))
            .ToArray();

        _genres.Clear();
        _genres.AddRange(distinctGenres);
    }
}
