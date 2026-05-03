using System.Net;
using SmartMovieCatalog.Api.Tests.TestSupport;

namespace SmartMovieCatalog.Api.Tests.Auth;

public sealed class CurrentUserUnauthorizedTests : IClassFixture<SmartMovieCatalogApiFactory>
{
    private readonly SmartMovieCatalogApiFactory _factory;

    public CurrentUserUnauthorizedTests(SmartMovieCatalogApiFactory factory)
    {
        _factory = factory;
        _factory.Users.Clear();
    }

    [Fact]
    public async Task Me_WithoutToken_ReturnsUnauthorized()
    {
        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync("/api/auth/me");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Me_WithMalformedToken_ReturnsUnauthorized()
    {
        HttpClient client = _factory.CreateClient();
        AuthApiFixture.SetBearerToken(client, "not-a-token");

        HttpResponseMessage response = await client.GetAsync("/api/auth/me");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Me_WithExpiredToken_ReturnsUnauthorized()
    {
        var user = AuthApiFixture.ActiveUser();
        _factory.Users.Add(user);
        HttpClient client = _factory.CreateClient();
        AuthApiFixture.SetBearerToken(client, AuthApiFixture.CreateToken(user.Id, DateTimeOffset.UtcNow.AddMinutes(-10)));

        HttpResponseMessage response = await client.GetAsync("/api/auth/me");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Me_WithIncorrectlySignedToken_ReturnsUnauthorized()
    {
        var user = AuthApiFixture.ActiveUser();
        _factory.Users.Add(user);
        HttpClient client = _factory.CreateClient();
        AuthApiFixture.SetBearerToken(client, AuthApiFixture.CreateToken(user.Id, signingKey: TestJwtOptions.WrongSigningKey));

        HttpResponseMessage response = await client.GetAsync("/api/auth/me");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
