using System.Net;
using System.Net.Http.Json;
using SmartMovieCatalog.Api.Tests.TestSupport;
using SmartMovieCatalog.Contracts.Movies;

namespace SmartMovieCatalog.Api.Tests.Movies;

public sealed class CreateMovieEndpointTests : IClassFixture<SmartMovieCatalogApiFactory>
{
    private readonly SmartMovieCatalogApiFactory _factory;

    public CreateMovieEndpointTests(SmartMovieCatalogApiFactory factory)
    {
        _factory = factory;
        _factory.Users.Clear();
        _factory.Movies.Clear();
    }

    [Fact]
    public async Task CreateMovie_WithValidRequest_ReturnsCreatedMovie()
    {
        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync(
            "/api/movies",
            ValidRequest(countryCode: "br"));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        MovieResponse? body = await response.Content.ReadFromJsonAsync<MovieResponse>();
        Assert.NotNull(body);
        Assert.True(Guid.TryParse(body.Id, out _));
        Assert.Equal("Central do Brasil", body.Title);
        Assert.Equal("BR", body.CountryCode);
        Assert.Equal(["Drama"], body.Genres);
        Assert.Equal(new Uri($"/api/movies/{body.Id}", UriKind.Relative), response.Headers.Location);
        Assert.Single(_factory.Movies.Movies);
    }

    [Fact]
    public async Task CreateMovie_WithoutAuthorizationHeader_DoesNotRequireAuthentication()
    {
        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync("/api/movies", ValidRequest());

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    public static CreateMovieRequest ValidRequest(string countryCode = "BR")
    {
        return new CreateMovieRequest
        {
            Title = "Central do Brasil",
            OriginalTitle = "Central do Brasil",
            ReleaseYear = 1998,
            CountryCode = countryCode,
            OriginalLanguage = "pt-BR",
            Genres = ["Drama"],
            Director = "Walter Salles",
            Synopsis = "A retired teacher and a young boy travel through Brazil in search of his father.",
            DurationMinutes = 110,
            AgeRating = "12"
        };
    }
}
