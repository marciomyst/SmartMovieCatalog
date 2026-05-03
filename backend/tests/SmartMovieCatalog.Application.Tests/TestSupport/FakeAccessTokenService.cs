using SmartMovieCatalog.Application.Abstractions.Authentication;
using SmartMovieCatalog.Domain.Users;

namespace SmartMovieCatalog.Application.Tests.TestSupport;

public sealed class FakeAccessTokenService : IAccessTokenService
{
    public AccessToken CreateAccessToken(User user, DateTimeOffset issuedAtUtc)
    {
        return new AccessToken("fake-token", issuedAtUtc.AddMinutes(60));
    }
}
