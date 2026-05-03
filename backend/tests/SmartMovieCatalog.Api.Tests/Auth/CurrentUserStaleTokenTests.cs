using System.Net;
using SmartMovieCatalog.Api.Tests.TestSupport;

namespace SmartMovieCatalog.Api.Tests.Auth;

public sealed class CurrentUserStaleTokenTests : IClassFixture<SmartMovieCatalogApiFactory>
{
    private readonly SmartMovieCatalogApiFactory _factory;

    public CurrentUserStaleTokenTests(SmartMovieCatalogApiFactory factory)
    {
        _factory = factory;
        _factory.Users.Clear();
    }

    [Fact]
    public async Task Me_WithMissingPersistedUser_ReturnsUnauthorized()
    {
        HttpClient client = _factory.CreateClient();
        AuthApiFixture.SetBearerToken(client, AuthApiFixture.CreateToken(Guid.NewGuid()));

        HttpResponseMessage response = await client.GetAsync("/api/auth/me");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Me_WithInactivePersistedUser_ReturnsUnauthorized()
    {
        var user = AuthApiFixture.ActiveUser();
        user.Deactivate(DateTimeOffset.UtcNow);
        _factory.Users.Add(user);
        HttpClient client = _factory.CreateClient();
        AuthApiFixture.SetBearerToken(client, AuthApiFixture.CreateToken(user.Id));

        HttpResponseMessage response = await client.GetAsync("/api/auth/me");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Me_WithRemovedPersistedUser_ReturnsUnauthorized()
    {
        var user = AuthApiFixture.ActiveUser();
        user.Remove(DateTimeOffset.UtcNow);
        _factory.Users.Add(user);
        HttpClient client = _factory.CreateClient();
        AuthApiFixture.SetBearerToken(client, AuthApiFixture.CreateToken(user.Id));

        HttpResponseMessage response = await client.GetAsync("/api/auth/me");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
