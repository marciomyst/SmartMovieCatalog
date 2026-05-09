using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using SmartMovieCatalog.Api.Tests.TestSupport;
using SmartMovieCatalog.Contracts.Movies;

namespace SmartMovieCatalog.Api.Tests.Movies;

public sealed class CreateMovieEndpointContractTests : IClassFixture<SmartMovieCatalogApiFactory>
{
    private readonly SmartMovieCatalogApiFactory _factory;

    public CreateMovieEndpointContractTests(SmartMovieCatalogApiFactory factory)
    {
        _factory = factory;
        _factory.Users.Clear();
        _factory.Movies.Clear();
        _factory.Genres.Clear();
    }

    [Fact]
    public async Task CreateMovie_ResponseUsesContractShape()
    {
        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync(
            "/api/movies",
            new CreateMovieRequest
            {
                Title = "Central do Brasil",
                ReleaseYear = 1998,
                CountryCode = "BR",
                OriginalLanguage = "pt-BR",
                ExternalId = 666,
                Image = "/p/example-card.jpg"
            });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        JsonElement root = document.RootElement;
        Assert.True(root.TryGetProperty("id", out JsonElement id));
        Assert.True(Guid.TryParse(id.GetString(), out _));
        Assert.Equal("Central do Brasil", root.GetProperty("title").GetString());
        Assert.Equal(JsonValueKind.Null, root.GetProperty("originalTitle").ValueKind);
        Assert.Equal(1998, root.GetProperty("releaseYear").GetInt32());
        Assert.Equal("BR", root.GetProperty("countryCode").GetString());
        Assert.Equal("pt-BR", root.GetProperty("originalLanguage").GetString());
        Assert.Equal(JsonValueKind.Array, root.GetProperty("genres").ValueKind);
        Assert.Equal(JsonValueKind.Null, root.GetProperty("director").ValueKind);
        Assert.Equal(JsonValueKind.Null, root.GetProperty("synopsis").ValueKind);
        Assert.Equal(JsonValueKind.Null, root.GetProperty("durationMinutes").ValueKind);
        Assert.Equal(JsonValueKind.Null, root.GetProperty("ageRating").ValueKind);
        Assert.Equal(666, root.GetProperty("externalId").GetInt32());
        Assert.Equal("/p/example-card.jpg", root.GetProperty("image").GetString());
    }

    [Fact]
    public async Task CreateMovie_LocationPointsToReadableResource()
    {
        HttpClient client = _factory.CreateClient();

        HttpResponseMessage createResponse = await client.PostAsJsonAsync(
            "/api/movies",
            CreateMovieEndpointTests.ValidRequest());

        MovieResponse? body = await createResponse.Content.ReadFromJsonAsync<MovieResponse>();
        Assert.NotNull(body);
        Assert.Equal(new Uri($"/api/movies/{body.Id}", UriKind.Relative), createResponse.Headers.Location);

        HttpResponseMessage readResponse = await client.GetAsync($"/api/movies/{body.Id}");
        Assert.Equal(HttpStatusCode.OK, readResponse.StatusCode);
    }

    [Fact]
    public async Task CreateMovie_WhenExternalMetadataOmitted_ReturnsNullExternalMetadata()
    {
        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync(
            "/api/movies",
            new CreateMovieRequest
            {
                Title = "Central do Brasil",
                ReleaseYear = 1998,
                CountryCode = "BR",
                OriginalLanguage = "pt-BR"
            });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        using JsonDocument document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        JsonElement root = document.RootElement;
        Assert.Equal(JsonValueKind.Null, root.GetProperty("externalId").ValueKind);
        Assert.Equal(JsonValueKind.Null, root.GetProperty("image").ValueKind);
    }
}
