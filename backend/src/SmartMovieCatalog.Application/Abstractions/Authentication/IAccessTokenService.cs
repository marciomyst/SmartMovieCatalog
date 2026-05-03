using SmartMovieCatalog.Domain.Users;

namespace SmartMovieCatalog.Application.Abstractions.Authentication;

public interface IAccessTokenService
{
    AccessToken CreateAccessToken(User user, DateTimeOffset issuedAtUtc);
}

public sealed record AccessToken(string Value, DateTimeOffset ExpiresAtUtc);
