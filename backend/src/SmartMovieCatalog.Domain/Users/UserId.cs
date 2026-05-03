using SmartMovieCatalog.Domain.Common;

namespace SmartMovieCatalog.Domain.Users;

public sealed class UserId : ValueObject
{
    private UserId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static UserId New() => new(Guid.NewGuid());

    public static UserId From(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(value));
        }

        return new UserId(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
