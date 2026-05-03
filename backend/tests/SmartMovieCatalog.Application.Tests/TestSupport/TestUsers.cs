using SmartMovieCatalog.Domain.Users;

namespace SmartMovieCatalog.Application.Tests.TestSupport;

public static class TestUsers
{
    public const string Password = "Password123!";

    public static User ActiveUser(string email = "user@example.com")
    {
        FakePasswordHasher passwordHasher = new();
        User user = User.Create(
            UserId.New(),
            EmailAddress.Create(email),
            "Example User",
            "pending",
            [UserRole.Create(UserRole.User)],
            mustChangePasswordOnFirstLogin: false,
            new DateTimeOffset(2026, 5, 3, 1, 0, 0, TimeSpan.Zero));

        user.SetPasswordHash(passwordHasher.HashPassword(user, Password), new DateTimeOffset(2026, 5, 3, 1, 0, 0, TimeSpan.Zero));
        return user;
    }
}
