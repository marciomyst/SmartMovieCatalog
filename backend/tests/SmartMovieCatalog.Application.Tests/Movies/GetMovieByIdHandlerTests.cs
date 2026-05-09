using SmartMovieCatalog.Application;
using SmartMovieCatalog.Application.Features.Movies;
using SmartMovieCatalog.Application.Tests.TestSupport;
using SmartMovieCatalog.Domain.Movies;

namespace SmartMovieCatalog.Application.Tests.Movies;

public sealed class GetMovieByIdHandlerTests
{
    [Fact]
    public async Task Handle_WithExistingMovie_ReturnsMovieDetails()
    {
        FakeMovieRepository movies = new();
        Genre drama = Genre.Create(GenreId.New(), "Drama", externalId: null);
        Movie movie = CreateMovie("Central do Brasil", drama);
        await movies.AddAsync(movie, CancellationToken.None);
        GetMovieByIdHandler handler = new(movies);

        Result<MovieDetails, GetMovieByIdFailure> result = await handler.Handle(
            new GetMovieByIdQuery(movie.Id),
            CancellationToken.None);

        Assert.True(result.IsSuccess);
        MovieDetails details = result.Value;
        Assert.Equal(movie.Id, details.Id);
        Assert.Equal("Central do Brasil", details.Title);
        Assert.Equal("BR", details.CountryCode);
        Assert.Equal(["Drama"], details.Genres);
        Assert.Equal("/p/central.jpg", details.PosterUrl);
    }

    [Fact]
    public async Task Handle_WithMissingMovie_ReturnsNotFoundFailure()
    {
        FakeMovieRepository movies = new();
        GetMovieByIdHandler handler = new(movies);

        Result<MovieDetails, GetMovieByIdFailure> result = await handler.Handle(
            new GetMovieByIdQuery(Guid.NewGuid()),
            CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(GetMovieByIdFailure.NotFound, result.Error);
    }

    private static Movie CreateMovie(string title, Genre genre)
    {
        return Movie.Create(
            MovieId.New(),
            title,
            "Central Station",
            1998,
            "BR",
            "pt-BR",
            [genre],
            "Walter Salles",
            "A retired teacher and a young boy travel through Brazil.",
            110,
            "12",
            666,
            "/p/central.jpg",
            new DateTimeOffset(2026, 5, 4, 12, 0, 0, TimeSpan.Zero));
    }
}
