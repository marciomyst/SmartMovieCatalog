using SmartMovieCatalog.Domain.Common;

namespace SmartMovieCatalog.Domain.Users;

public sealed class UserRole : ValueObject
{
    public const string Admin = "Admin";
    public const string User = "User";

    private static readonly HashSet<string> AllowedRoles = new(StringComparer.Ordinal)
    {
        Admin,
        User
    };

    private UserRole()
    {
        Name = User;
    }

    private UserRole(string name)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public static UserRole Create(string name)
    {
        string role = Guard.AgainstNullOrWhiteSpace(name, nameof(name)).Trim();

        if (!AllowedRoles.Contains(role))
        {
            throw new ArgumentException("User role is not recognized.", nameof(name));
        }

        return new UserRole(role);
    }

    public static IReadOnlyCollection<string> Allowed => AllowedRoles.ToArray();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
    }

    public override string ToString() => Name;
}
