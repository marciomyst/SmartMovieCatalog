using SmartMovieCatalog.Application.Features.Movies;
using SmartMovieCatalog.Application.Tests.TestSupport;
using SmartMovieCatalog.Domain.Movies;

namespace SmartMovieCatalog.Application.Tests.Movies;

public sealed class ListMoviesHandlerTests
{
    [Fact]
    public async Task Handle_WithPagedQuery_ReturnsMovieSummaries()
    {
        FakeMovieRepository movies = new();
        Genre zulu = Genre.Create(GenreId.New(), "Zulu", externalId: null);
        Genre alpha = Genre.Create(GenreId.New(), "Alpha", externalId: null);
        Movie firstMovie = CreateMovie("Central do Brasil", "Central Station", new DateTimeOffset(2026, 5, 4, 12, 0, 0, TimeSpan.Zero), zulu, alpha);
        Movie secondMovie = CreateMovie("Ainda Estou Aqui", null, new DateTimeOffset(2026, 5, 5, 12, 0, 0, TimeSpan.Zero), alpha);
        await movies.AddAsync(firstMovie, CancellationToken.None);
        await movies.AddAsync(secondMovie, CancellationToken.None);
        ListMoviesHandler handler = new(movies);

        PagedMovieSummaries result = await handler.Handle(
            new ListMoviesQuery("central", 1, 12),
            CancellationToken.None);

        MovieSummary summary = Assert.Single(result.Items);
        Assert.Equal(firstMovie.Id, summary.Id);
        Assert.Equal("Central do Brasil", summary.Title);
        Assert.Equal(["Alpha", "Zulu"], summary.Genres);
        Assert.Equal("/p/central.jpg", summary.PosterUrl);
        Assert.Equal(firstMovie.CreatedAtUtc, summary.CreatedAt);
        Assert.Equal(1, result.Page);
        Assert.Equal(12, result.PageSize);
        Assert.Equal(1, result.TotalCount);
        Assert.Equal(1, result.TotalPages);
        Assert.False(result.HasPreviousPage);
        Assert.False(result.HasNextPage);
    }

    private static Movie CreateMovie(
        string title,
        string? originalTitle,
        DateTimeOffset createdAtUtc,
        params Genre[] genres)
    {
        return Movie.Create(
            MovieId.New(),
            title,
            originalTitle,
            1998,
            "BR",
            "pt-BR",
            genres,
            "Walter Salles",
            synopsis: null,
            durationMinutes: null,
            ageRating: null,
            externalId: null,
            image: "/p/central.jpg",
            createdAtUtc);
    }
}
