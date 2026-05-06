using SmartMovieCatalog.Domain.Common;

namespace SmartMovieCatalog.Domain.Movies;

public sealed class MovieId : ValueObject
{
    private MovieId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static MovieId New() => new(Guid.NewGuid());

    public static MovieId From(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Movie id cannot be empty.", nameof(value));
        }

        return new MovieId(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
