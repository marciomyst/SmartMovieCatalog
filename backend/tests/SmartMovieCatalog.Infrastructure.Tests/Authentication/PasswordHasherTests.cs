using SmartMovieCatalog.Domain.Users;
using SmartMovieCatalog.Infrastructure.Authentication;

namespace SmartMovieCatalog.Infrastructure.Tests.Authentication;

public sealed class PasswordHasherTests
{
    [Fact]
    public void HashPassword_WithValidPassword_CreatesVerifiableHash()
    {
        PasswordHasher passwordHasher = new();
        User user = CreateUser();

        string hash = passwordHasher.HashPassword(user, "Password123!");
        user.SetPasswordHash(hash, DateTimeOffset.UtcNow);

        Assert.StartsWith("PBKDF2-SHA256$100000$", hash, StringComparison.Ordinal);
        Assert.True(passwordHasher.VerifyPassword(user, "Password123!"));
        Assert.False(passwordHasher.VerifyPassword(user, "wrong"));
    }

    [Fact]
    public void VerifyPassword_WithMalformedHash_ReturnsFalse()
    {
        PasswordHasher passwordHasher = new();
        User user = CreateUser();
        user.SetPasswordHash("not-a-valid-hash", DateTimeOffset.UtcNow);

        Assert.False(passwordHasher.VerifyPassword(user, "Password123!"));
    }

    [Fact]
    public void VerifyPassword_WithInvalidBase64Hash_ReturnsFalse()
    {
        PasswordHasher passwordHasher = new();
        User user = CreateUser();
        user.SetPasswordHash("PBKDF2-SHA256$100000$not-base64$also-not-base64", DateTimeOffset.UtcNow);

        Assert.False(passwordHasher.VerifyPassword(user, "Password123!"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void VerifyPassword_WithInvalidPassword_ReturnsFalse(string? password)
    {
        PasswordHasher passwordHasher = new();
        User user = CreateUser();
        user.SetPasswordHash(passwordHasher.HashPassword(user, "Password123!"), DateTimeOffset.UtcNow);

        Assert.False(passwordHasher.VerifyPassword(user, password!));
    }

    private static User CreateUser()
    {
        return User.Create(
            UserId.New(),
            EmailAddress.Create("user@example.com"),
            "Example User",
            "pending",
            [UserRole.Create(UserRole.User)],
            mustChangePasswordOnFirstLogin: false,
            DateTimeOffset.UtcNow);
    }
}
