using System.Net;
using System.Net.Http.Json;
using SmartMovieCatalog.Api.Tests.TestSupport;
using SmartMovieCatalog.Contracts.Auth;

namespace SmartMovieCatalog.Api.Tests.Auth;

public sealed class CurrentUserEndpointTests : IClassFixture<SmartMovieCatalogApiFactory>
{
    private readonly SmartMovieCatalogApiFactory _factory;

    public CurrentUserEndpointTests(SmartMovieCatalogApiFactory factory)
    {
        _factory = factory;
        _factory.Users.Clear();
    }

    [Fact]
    public async Task Me_WithValidToken_ReturnsCurrentUserContext()
    {
        var user = AuthApiFixture.ActiveUser();
        _factory.Users.Add(user);
        HttpClient client = _factory.CreateClient();
        AuthApiFixture.SetBearerToken(client, AuthApiFixture.CreateToken(user.Id));

        HttpResponseMessage response = await client.GetAsync("/api/auth/me");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        CurrentUserResponse? body = await response.Content.ReadFromJsonAsync<CurrentUserResponse>();
        Assert.NotNull(body);
        Assert.Equal(user.Id, body.UserId);
        Assert.Equal("user@example.com", body.Email);
        Assert.Equal("Example User", body.Name);
        Assert.Equal(["User"], body.Roles);
        Assert.False(body.MustChangePasswordOnFirstLogin);
    }
}
