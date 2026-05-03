using SmartMovieCatalog.Application.Abstractions.Authentication;
using SmartMovieCatalog.Domain.Users;

namespace SmartMovieCatalog.Application.Tests.TestSupport;

public sealed class FakePasswordHasher : IPasswordHasher
{
    public string HashPassword(User user, string password)
    {
        return $"fake:{password}";
    }

    public bool VerifyPassword(User user, string password)
    {
        return user.PasswordHash == HashPassword(user, password);
    }
}
