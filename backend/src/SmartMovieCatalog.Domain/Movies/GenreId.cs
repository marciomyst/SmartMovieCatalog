using SmartMovieCatalog.Domain.Common;

namespace SmartMovieCatalog.Domain.Movies;

public sealed class GenreId : ValueObject
{
    private GenreId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static GenreId New() => new(Guid.NewGuid());

    public static GenreId From(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Genre id cannot be empty.", nameof(value));
        }

        return new GenreId(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
