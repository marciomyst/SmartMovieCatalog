using SmartMovieCatalog.Application.Abstractions.Authentication;
using SmartMovieCatalog.Domain.Users;

namespace SmartMovieCatalog.Api.Tests.TestSupport;

public sealed class TestPasswordHasher : IPasswordHasher
{
    public string HashPassword(User user, string password)
    {
        return $"test:{password}";
    }

    public bool VerifyPassword(User user, string password)
    {
        return user.PasswordHash == HashPassword(user, password);
    }
}
