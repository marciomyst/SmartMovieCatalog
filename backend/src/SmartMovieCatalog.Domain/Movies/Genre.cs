using SmartMovieCatalog.Domain.Common;

namespace SmartMovieCatalog.Domain.Movies;

public sealed class Genre : AggregateRoot
{
    private Genre()
    {
        Name = "Unknown";
        NormalizedName = NormalizeName(Name);
    }

    private Genre(GenreId id, string name, int? externalId)
    {
        Id = id.Value;
        Name = NormalizeDisplayName(name);
        NormalizedName = NormalizeName(Name);
        ExternalId = ValidateExternalId(externalId);
    }

    public string Name { get; private set; }

    public string NormalizedName { get; private set; }

    public int? ExternalId { get; private set; }

    public static Genre Create(GenreId id, string? name, int? externalId)
    {
        return new Genre(id, name!, externalId);
    }

    public static string NormalizeName(string? name)
    {
        return NormalizeDisplayName(name).ToUpperInvariant();
    }

    private static string NormalizeDisplayName(string? name)
    {
        return Guard.AgainstNullOrWhiteSpace(name, nameof(name)).Trim();
    }

    private static int? ValidateExternalId(int? externalId)
    {
        if (externalId <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(externalId),
                externalId,
                "Genre external ID must be positive when supplied.");
        }

        return externalId;
    }
}
