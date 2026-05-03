using SmartMovieCatalog.Domain.Common;

namespace SmartMovieCatalog.Domain.Users;

public sealed class User : AggregateRoot
{
    private readonly List<UserRole> _roles = [];

    private User()
    {
        Email = EmailAddress.Create("placeholder@example.com");
        NormalizedEmail = Email.NormalizedValue;
        Name = "Placeholder";
        PasswordHash = "not-set";
        CreatedAtUtc = DateTimeOffset.UnixEpoch;
    }

    private User(
        UserId id,
        EmailAddress email,
        string name,
        string passwordHash,
        IEnumerable<UserRole> roles,
        bool mustChangePasswordOnFirstLogin,
        DateTimeOffset createdAtUtc)
    {
        Id = id.Value;
        Email = email;
        NormalizedEmail = email.NormalizedValue;
        Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name)).Trim();
        PasswordHash = Guard.AgainstNullOrWhiteSpace(passwordHash, nameof(passwordHash));
        MustChangePasswordOnFirstLogin = mustChangePasswordOnFirstLogin;
        CreatedAtUtc = createdAtUtc;
        IsActive = true;
        ReplaceRoles(roles);
    }

    public EmailAddress Email { get; private set; }

    public string NormalizedEmail { get; private set; }

    public string Name { get; private set; }

    public string PasswordHash { get; private set; }

    public bool IsActive { get; private set; }

    public DateTimeOffset? RemovedAtUtc { get; private set; }

    public bool MustChangePasswordOnFirstLogin { get; private set; }

    public DateTimeOffset CreatedAtUtc { get; private set; }

    public DateTimeOffset? UpdatedAtUtc { get; private set; }

    public IReadOnlyCollection<UserRole> Roles => _roles.AsReadOnly();

    public bool CanAuthenticate => IsActive && RemovedAtUtc is null;

    public static User Create(
        UserId id,
        EmailAddress email,
        string name,
        string passwordHash,
        IEnumerable<UserRole> roles,
        bool mustChangePasswordOnFirstLogin,
        DateTimeOffset createdAtUtc)
    {
        return new User(id, email, name, passwordHash, roles, mustChangePasswordOnFirstLogin, createdAtUtc);
    }

    public void Deactivate(DateTimeOffset updatedAtUtc)
    {
        IsActive = false;
        UpdatedAtUtc = updatedAtUtc;
    }

    public void Remove(DateTimeOffset removedAtUtc)
    {
        RemovedAtUtc = removedAtUtc;
        UpdatedAtUtc = removedAtUtc;
    }

    public void SetPasswordHash(string passwordHash, DateTimeOffset updatedAtUtc)
    {
        PasswordHash = Guard.AgainstNullOrWhiteSpace(passwordHash, nameof(passwordHash));
        UpdatedAtUtc = updatedAtUtc;
    }

    private void ReplaceRoles(IEnumerable<UserRole> roles)
    {
        UserRole[] distinctRoles = roles
            .Distinct()
            .ToArray();

        if (distinctRoles.Length == 0)
        {
            throw new ArgumentException("A user must have at least one role.", nameof(roles));
        }

        _roles.Clear();
        _roles.AddRange(distinctRoles);
    }
}
