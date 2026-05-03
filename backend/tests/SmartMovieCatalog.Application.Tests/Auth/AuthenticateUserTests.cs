using SmartMovieCatalog.Application.Features.Auth;
using SmartMovieCatalog.Application.Tests.TestSupport;

namespace SmartMovieCatalog.Application.Tests.Auth;

public sealed class AuthenticateUserTests
{
    [Fact]
    public async Task AuthenticateAsync_WithValidCredentials_ReturnsAccessToken()
    {
        FakeUserRepository users = new();
        users.Add(TestUsers.ActiveUser());

        AuthenticateUser useCase = new(
            users,
            new FakePasswordHasher(),
            new FakeAccessTokenService(),
            new FakeClock());

        AuthenticationResult result = await useCase.AuthenticateAsync(
            " USER@example.com ",
            TestUsers.Password,
            CancellationToken.None);

        Assert.True(result.Succeeded);
        Assert.NotEqual(Guid.Empty, result.UserId);
        Assert.Equal("user@example.com", result.Email);
        Assert.Equal("fake-token", result.AccessToken);
        Assert.Equal(new DateTimeOffset(2026, 5, 3, 3, 0, 0, TimeSpan.Zero), result.AccessTokenExpiresAtUtc);
    }
}
