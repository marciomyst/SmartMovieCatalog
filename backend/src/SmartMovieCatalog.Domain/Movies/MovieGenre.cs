using SmartMovieCatalog.Domain.Common;

namespace SmartMovieCatalog.Domain.Movies;

public sealed class MovieGenre : ValueObject
{
    private MovieGenre()
    {
        Name = "Unknown";
    }

    private MovieGenre(string name)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public static MovieGenre Create(string? name)
    {
        string normalizedName = Guard.AgainstNullOrWhiteSpace(name, nameof(name)).Trim();

        return new MovieGenre(normalizedName);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
    }

    public override string ToString() => Name;
}
