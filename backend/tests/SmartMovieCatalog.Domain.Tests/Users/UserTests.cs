using SmartMovieCatalog.Domain.Users;

namespace SmartMovieCatalog.Domain.Tests.Users;

public sealed class UserTests
{
    [Fact]
    public void Create_WithValidValues_CreatesActiveUser()
    {
        DateTimeOffset createdAtUtc = new(2026, 5, 3, 1, 0, 0, TimeSpan.Zero);
        User user = CreateUser(createdAtUtc: createdAtUtc);

        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal("user@example.com", user.Email.Value);
        Assert.Equal("USER@EXAMPLE.COM", user.NormalizedEmail);
        Assert.Equal("Example User", user.Name);
        Assert.Equal("hash", user.PasswordHash);
        Assert.True(user.IsActive);
        Assert.True(user.CanAuthenticate);
        Assert.False(user.MustChangePasswordOnFirstLogin);
        Assert.Equal(createdAtUtc, user.CreatedAtUtc);
        Assert.Contains(user.Roles, role => role.Name == UserRole.User);
    }

    [Fact]
    public void Create_WithoutRoles_Throws()
    {
        Assert.Throws<ArgumentException>(() => User.Create(
            UserId.New(),
            EmailAddress.Create("user@example.com"),
            "Example User",
            "hash",
            [],
            mustChangePasswordOnFirstLogin: false,
            DateTimeOffset.UtcNow));
    }

    [Fact]
    public void Create_WithDuplicateRoles_KeepsDistinctRoles()
    {
        User user = User.Create(
            UserId.New(),
            EmailAddress.Create("user@example.com"),
            "Example User",
            "hash",
            [UserRole.Create(UserRole.User), UserRole.Create(UserRole.User)],
            mustChangePasswordOnFirstLogin: false,
            DateTimeOffset.UtcNow);

        Assert.Single(user.Roles);
    }

    [Fact]
    public void Deactivate_MarksUserAsUnavailableForAuthentication()
    {
        User user = CreateUser();
        DateTimeOffset updatedAtUtc = new(2026, 5, 3, 2, 0, 0, TimeSpan.Zero);

        user.Deactivate(updatedAtUtc);

        Assert.False(user.IsActive);
        Assert.False(user.CanAuthenticate);
        Assert.Equal(updatedAtUtc, user.UpdatedAtUtc);
    }

    [Fact]
    public void Remove_MarksUserAsUnavailableForAuthentication()
    {
        User user = CreateUser();
        DateTimeOffset removedAtUtc = new(2026, 5, 3, 2, 0, 0, TimeSpan.Zero);

        user.Remove(removedAtUtc);

        Assert.Equal(removedAtUtc, user.RemovedAtUtc);
        Assert.Equal(removedAtUtc, user.UpdatedAtUtc);
        Assert.False(user.CanAuthenticate);
    }

    [Fact]
    public void SetPasswordHash_UpdatesPasswordHashAndTimestamp()
    {
        User user = CreateUser();
        DateTimeOffset updatedAtUtc = new(2026, 5, 3, 2, 0, 0, TimeSpan.Zero);

        user.SetPasswordHash("new-hash", updatedAtUtc);

        Assert.Equal("new-hash", user.PasswordHash);
        Assert.Equal(updatedAtUtc, user.UpdatedAtUtc);
    }

    private static User CreateUser(DateTimeOffset? createdAtUtc = null)
    {
        return User.Create(
            UserId.New(),
            EmailAddress.Create("user@example.com"),
            "Example User",
            "hash",
            [UserRole.Create(UserRole.User)],
            mustChangePasswordOnFirstLogin: false,
            createdAtUtc ?? DateTimeOffset.UtcNow);
    }
}
