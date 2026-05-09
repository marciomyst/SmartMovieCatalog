using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using SmartMovieCatalog.Api.Tests.TestSupport;
using SmartMovieCatalog.Contracts.Movies;

namespace SmartMovieCatalog.Api.Tests.Movies;

public sealed class GetMovieByIdEndpointTests : IClassFixture<SmartMovieCatalogApiFactory>
{
    private readonly SmartMovieCatalogApiFactory _factory;

    public GetMovieByIdEndpointTests(SmartMovieCatalogApiFactory factory)
    {
        _factory = factory;
        _factory.Users.Clear();
        _factory.Movies.Clear();
        _factory.Genres.Clear();
    }

    [Fact]
    public async Task GetMovieById_WithExistingMovie_ReturnsMovieDetails()
    {
        HttpClient client = _factory.CreateClient();
        HttpResponseMessage createResponse = await client.PostAsJsonAsync(
            "/api/movies",
            CreateMovieEndpointTests.ValidRequest() with
            {
                ExternalId = 666,
                Image = "/p/central.jpg"
            });

        MovieResponse? createdMovie = await createResponse.Content.ReadFromJsonAsync<MovieResponse>();
        Assert.NotNull(createdMovie);

        HttpResponseMessage response = await client.GetAsync($"/api/movies/{createdMovie.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
        MovieDetailsResponse? details = await response.Content.ReadFromJsonAsync<MovieDetailsResponse>();
        Assert.NotNull(details);
        Assert.Equal(createdMovie.Id, details.Id);
        Assert.Equal("Central do Brasil", details.Title);
        Assert.Equal("BR", details.CountryCode);
        Assert.Equal("pt-BR", details.OriginalLanguage);
        Assert.Equal(["Drama"], details.Genres);
        Assert.Equal(666, details.ExternalId);
        Assert.Equal("/p/central.jpg", details.PosterUrl);
        Assert.NotEqual(default, details.CreatedAt);
    }

    [Fact]
    public async Task GetMovieById_WithInvalidId_ReturnsBadRequestProblemDetails()
    {
        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync("/api/movies/not-a-guid");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        ProblemDetails? problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problemDetails);
        Assert.Equal(400, problemDetails.Status);
        Assert.False(string.IsNullOrWhiteSpace(problemDetails.Type));
        Assert.False(string.IsNullOrWhiteSpace(problemDetails.Title));
        Assert.False(string.IsNullOrWhiteSpace(problemDetails.Detail));
        Assert.False(string.IsNullOrWhiteSpace(problemDetails.Instance));
    }

    [Fact]
    public async Task GetMovieById_WithUnknownId_ReturnsNotFoundProblemDetails()
    {
        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync($"/api/movies/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        ProblemDetails? problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.NotNull(problemDetails);
        Assert.Equal(404, problemDetails.Status);
        Assert.False(string.IsNullOrWhiteSpace(problemDetails.Type));
        Assert.False(string.IsNullOrWhiteSpace(problemDetails.Title));
        Assert.False(string.IsNullOrWhiteSpace(problemDetails.Detail));
        Assert.False(string.IsNullOrWhiteSpace(problemDetails.Instance));
    }
}
