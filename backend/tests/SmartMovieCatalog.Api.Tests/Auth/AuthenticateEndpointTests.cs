using System.Net;
using System.Net.Http.Json;
using SmartMovieCatalog.Api.Tests.TestSupport;
using SmartMovieCatalog.Contracts.Auth;

namespace SmartMovieCatalog.Api.Tests.Auth;

public sealed class AuthenticateEndpointTests : IClassFixture<SmartMovieCatalogApiFactory>
{
    private readonly SmartMovieCatalogApiFactory _factory;

    public AuthenticateEndpointTests(SmartMovieCatalogApiFactory factory)
    {
        _factory = factory;
        _factory.Users.Clear();
    }

    [Fact]
    public async Task Authenticate_WithValidCredentials_ReturnsToken()
    {
        var user = AuthApiFixture.ActiveUser();
        _factory.Users.Add(user);
        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync(
            "/api/auth/authenticate",
            new AuthenticateRequest { Email = " USER@example.com ", Password = AuthApiFixture.Password });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        AuthenticateResponse? body = await response.Content.ReadFromJsonAsync<AuthenticateResponse>();
        Assert.NotNull(body);
        Assert.Equal(user.Id, body.UserId);
        Assert.Equal("user@example.com", body.Email);
        Assert.False(string.IsNullOrWhiteSpace(body.AccessToken));
        Assert.True(body.AccessTokenExpiresAtUtc > DateTimeOffset.UtcNow.AddMinutes(30));
    }
}
