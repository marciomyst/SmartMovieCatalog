using SmartMovieCatalog.Application.Features.Movies;
using SmartMovieCatalog.Application.Tests.TestSupport;

namespace SmartMovieCatalog.Application.Tests.Movies;

public sealed class CreateMovieHandlerTests
{
    [Fact]
    public async Task Handle_WithValidCommand_PersistsMovieAndReturnsCreatedMovie()
    {
        FakeMovieRepository movies = new();
        FakeClock clock = new()
        {
            UtcNow = new DateTimeOffset(2026, 5, 4, 12, 0, 0, TimeSpan.Zero)
        };
        CreateMovieHandler handler = new(movies, clock);

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
                " 12 "),
            CancellationToken.None);

        Assert.NotEqual(Guid.Empty, createdMovie.Id);
        Assert.Equal("Central do Brasil", createdMovie.Title);
        Assert.Equal("BR", createdMovie.CountryCode);
        Assert.Equal(["Drama"], createdMovie.Genres);
        Assert.Single(movies.Movies);
        Assert.Equal(clock.UtcNow, movies.Movies.Single().CreatedAtUtc);
    }
}
