using SmartMovieCatalog.Application.Features.Movies;
using SmartMovieCatalog.Application.Tests.TestSupport;
using SmartMovieCatalog.Domain.Movies;

namespace SmartMovieCatalog.Application.Tests.Movies;

public sealed class CreateMovieHandlerTests
{
    [Fact]
    public async Task Handle_WithValidCommand_PersistsMovieAndReturnsCreatedMovie()
    {
        FakeMovieRepository movies = new();
        FakeGenreRepository genres = new();
        FakeClock clock = new()
        {
            UtcNow = new DateTimeOffset(2026, 5, 4, 12, 0, 0, TimeSpan.Zero)
        };
        CreateMovieHandler handler = new(movies, genres, clock);

        CreatedMovie createdMovie = await handler.Handle(
            new CreateMovieCommand(
                " Central do Brasil ",
                " Central do Brasil ",
                1998,
                " br ",
                " pt-BR ",
                [" Drama "],
                " Walter Salles ",
                " A retired teacher and a young boy travel through Brazil. ",
                110,
                " 12 ",
                666,
                " /p/example-card.jpg "),
            CancellationToken.None);

        Assert.NotEqual(Guid.Empty, createdMovie.Id);
        Assert.Equal("Central do Brasil", createdMovie.Title);
        Assert.Equal("BR", createdMovie.CountryCode);
        Assert.Equal(["Drama"], createdMovie.Genres);
        Assert.Equal(666, createdMovie.ExternalId);
        Assert.Equal("/p/example-card.jpg", createdMovie.Image);
        Assert.Single(movies.Movies);
        Assert.Equal(clock.UtcNow, movies.Movies.Single().CreatedAtUtc);
        Assert.Equal(666, movies.Movies.Single().ExternalId);
        Assert.Equal("/p/example-card.jpg", movies.Movies.Single().Image);
        Assert.Single(genres.Genres);
        Assert.Equal("Drama", genres.Genres.Single().Name);
        Assert.Null(genres.Genres.Single().ExternalId);
    }

    [Fact]
    public async Task Handle_WithExistingGenre_ReusesGenre()
    {
        FakeMovieRepository movies = new();
        FakeGenreRepository genres = new();
        Genre existingGenre = Genre.Create(GenreId.New(), "Drama", externalId: 18);
        genres.Add(existingGenre);
        CreateMovieHandler handler = new(movies, genres, new FakeClock());

        CreatedMovie createdMovie = await handler.Handle(
            new CreateMovieCommand(
                "Central do Brasil",
                null,
                1998,
                "BR",
                "pt-BR",
                [" drama "],
                null,
                null,
                null,
                null,
                null,
                null),
            CancellationToken.None);

        Assert.Equal(["Drama"], createdMovie.Genres);
        Assert.Single(genres.Genres);
        Assert.Same(existingGenre, movies.Movies.Single().Genres.Single().Genre);
    }

    [Fact]
    public async Task Handle_WithDuplicateEquivalentGenreNames_ReturnsOneGenre()
    {
        FakeMovieRepository movies = new();
        FakeGenreRepository genres = new();
        CreateMovieHandler handler = new(movies, genres, new FakeClock());

        CreatedMovie createdMovie = await handler.Handle(
            new CreateMovieCommand(
                "Central do Brasil",
                null,
                1998,
                "BR",
                "pt-BR",
                [" Drama ", "drama"],
                null,
                null,
                null,
                null,
                null,
                null),
            CancellationToken.None);

        Assert.Equal(["Drama"], createdMovie.Genres);
        Assert.Single(genres.Genres);
        Assert.Single(movies.Movies.Single().Genres);
    }
}
