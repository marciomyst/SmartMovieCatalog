using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using SmartMovieCatalog.Api.Tests.TestSupport;
using SmartMovieCatalog.Contracts.Auth;

namespace SmartMovieCatalog.Api.Tests.Auth;

public sealed class AuthenticateEndpointFailureTests : IClassFixture<SmartMovieCatalogApiFactory>
{
    private readonly SmartMovieCatalogApiFactory _factory;

    public AuthenticateEndpointFailureTests(SmartMovieCatalogApiFactory factory)
    {
        _factory = factory;
        _factory.Users.Clear();
    }

    [Fact]
    public async Task Authenticate_WithInvalidRequest_ReturnsValidationProblem()
    {
        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync(
            "/api/auth/authenticate",
            new AuthenticateRequest { Email = "not-an-email", Password = string.Empty });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        ValidationProblemDetails? problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal(400, problem.Status);
        Assert.True(problem.Errors.ContainsKey(nameof(AuthenticateRequest.Email)));
    }

    [Fact]
    public async Task Authenticate_WithWrongPassword_ReturnsGenericUnauthorizedProblem()
    {
        _factory.Users.Add(AuthApiFixture.ActiveUser());
        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync(
            "/api/auth/authenticate",
            new AuthenticateRequest { Email = "user@example.com", Password = "wrong" });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        ProblemDetails? problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal("Authentication failed.", problem.Title);
    }

    [Fact]
    public async Task Authenticate_WithNonexistentUser_ReturnsGenericUnauthorizedProblem()
    {
        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync(
            "/api/auth/authenticate",
            new AuthenticateRequest { Email = "missing@example.com", Password = AuthApiFixture.Password });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Authenticate_WithInactiveUser_ReturnsGenericUnauthorizedProblem()
    {
        var user = AuthApiFixture.ActiveUser();
        user.Deactivate(DateTimeOffset.UtcNow);
        _factory.Users.Add(user);
        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync(
            "/api/auth/authenticate",
            new AuthenticateRequest { Email = "user@example.com", Password = AuthApiFixture.Password });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Authenticate_WithRemovedUser_ReturnsGenericUnauthorizedProblem()
    {
        var user = AuthApiFixture.ActiveUser();
        user.Remove(DateTimeOffset.UtcNow);
        _factory.Users.Add(user);
        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync(
            "/api/auth/authenticate",
            new AuthenticateRequest { Email = "user@example.com", Password = AuthApiFixture.Password });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
