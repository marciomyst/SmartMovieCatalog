using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using SmartMovieCatalog.Api.Features.Movies.ListMovies;
using SmartMovieCatalog.Api.Tests.TestSupport;
using SmartMovieCatalog.Contracts.Movies;

namespace SmartMovieCatalog.Api.Tests.Movies;

public sealed class ListMoviesEndpointTests : IClassFixture<SmartMovieCatalogApiFactory>
{
    private readonly SmartMovieCatalogApiFactory _factory;

    public ListMoviesEndpointTests(SmartMovieCatalogApiFactory factory)
    {
        _factory = factory;
        _factory.Users.Clear();
        _factory.Movies.Clear();
        _factory.Genres.Clear();
    }

    [Fact]
    public async Task ListMovies_WithPaging_ReturnsPagedMovieSummaries()
    {
        HttpClient client = _factory.CreateClient();
        await client.PostAsJsonAsync("/api/movies", CreateMovieEndpointTests.ValidRequest() with { Image = "/p/central.jpg" });
        await client.PostAsJsonAsync(
            "/api/movies",
            CreateMovieEndpointTests.ValidRequest() with
            {
                Title = "Ainda Estou Aqui",
                OriginalTitle = null,
                Image = "/p/ainda-estou-aqui.jpg"
            });

        HttpResponseMessage response = await client.GetAsync("/api/movies?page=1&pageSize=1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        PagedMovieSummaryResponse? body = await response.Content.ReadFromJsonAsync<PagedMovieSummaryResponse>();
        Assert.NotNull(body);
        Assert.Single(body.Items);
        Assert.Equal(1, body.Page);
        Assert.Equal(1, body.PageSize);
        Assert.Equal(2, body.TotalCount);
        Assert.Equal(2, body.TotalPages);
        Assert.False(body.HasPreviousPage);
        Assert.True(body.HasNextPage);
        Assert.False(string.IsNullOrWhiteSpace(body.Items.Single().Id));
        Assert.NotEqual(default, body.Items.Single().CreatedAt);
    }

    [Fact]
    public async Task ListMovies_WithQuery_ReturnsMatchingTitle()
    {
        HttpClient client = _factory.CreateClient();
        await client.PostAsJsonAsync("/api/movies", CreateMovieEndpointTests.ValidRequest() with { Image = "/p/central.jpg" });
        await client.PostAsJsonAsync(
            "/api/movies",
            CreateMovieEndpointTests.ValidRequest() with
            {
                Title = "Ainda Estou Aqui",
                OriginalTitle = null,
                Image = "/p/ainda-estou-aqui.jpg"
            });

        HttpResponseMessage response = await client.GetAsync("/api/movies?query=central&page=1&pageSize=12");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        PagedMovieSummaryResponse? body = await response.Content.ReadFromJsonAsync<PagedMovieSummaryResponse>();
        Assert.NotNull(body);
        MovieSummaryResponse item = Assert.Single(body.Items);
        Assert.Equal("Central do Brasil", item.Title);
        Assert.Equal("BR", item.CountryCode);
        Assert.Equal(["Drama"], item.Genres);
        Assert.Equal("Walter Salles", item.Director);
        Assert.Equal("/p/central.jpg", item.PosterUrl);
    }

    [Fact]
    public async Task ListMovies_WithInvalidPaging_ReturnsValidationProblem()
    {
        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync("/api/movies?page=0&pageSize=0");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        ValidationProblemDetails? problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(problem);
        Assert.True(problem.Errors.ContainsKey(nameof(ListMoviesRequest.Page)));
        Assert.True(problem.Errors.ContainsKey(nameof(ListMoviesRequest.PageSize)));
    }
}
