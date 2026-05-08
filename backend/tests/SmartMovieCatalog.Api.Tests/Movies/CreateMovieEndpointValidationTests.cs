using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using SmartMovieCatalog.Api.Tests.TestSupport;
using SmartMovieCatalog.Contracts.Movies;

namespace SmartMovieCatalog.Api.Tests.Movies;

public sealed class CreateMovieEndpointValidationTests : IClassFixture<SmartMovieCatalogApiFactory>
{
    private readonly SmartMovieCatalogApiFactory _factory;

    public CreateMovieEndpointValidationTests(SmartMovieCatalogApiFactory factory)
    {
        _factory = factory;
        _factory.Users.Clear();
        _factory.Movies.Clear();
        _factory.Genres.Clear();
    }

    [Fact]
    public async Task CreateMovie_WithoutRequestBody_ReturnsValidationProblem()
    {
        HttpClient client = _factory.CreateClient();
        using HttpRequestMessage request = new(HttpMethod.Post, "/api/movies");
        request.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        ValidationProblemDetails? problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(problem);
        Assert.True(problem.Errors.ContainsKey(string.Empty));
        Assert.Empty(_factory.Movies.Movies);
    }

    [Fact]
    public async Task CreateMovie_WithInvalidRequest_ReturnsValidationProblem()
    {
        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync(
            "/api/movies",
            new CreateMovieRequest
            {
                Title = " ",
                ReleaseYear = 1887,
                CountryCode = "BRA",
                OriginalLanguage = " ",
                Genres = [" "],
                DurationMinutes = -1
            });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        ValidationProblemDetails? problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(problem);
        Assert.Equal(400, problem.Status);
        Assert.True(problem.Errors.ContainsKey(nameof(CreateMovieRequest.Title)));
        Assert.True(problem.Errors.ContainsKey(nameof(CreateMovieRequest.ReleaseYear)));
        Assert.True(problem.Errors.ContainsKey(nameof(CreateMovieRequest.CountryCode)));
        Assert.True(problem.Errors.ContainsKey(nameof(CreateMovieRequest.OriginalLanguage)));
        Assert.True(problem.Errors.ContainsKey("Genres[0]"));
        Assert.True(problem.Errors.ContainsKey(nameof(CreateMovieRequest.DurationMinutes)));
        Assert.Empty(_factory.Movies.Movies);
    }
}
