using SmartMovieCatalog.Domain.Users;

namespace SmartMovieCatalog.Application.Abstractions.Authentication;

public interface IPasswordHasher
{
    string HashPassword(User user, string password);

    bool VerifyPassword(User user, string password);
}
