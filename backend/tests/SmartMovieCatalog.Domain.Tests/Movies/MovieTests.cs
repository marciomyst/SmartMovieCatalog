using SmartMovieCatalog.Domain.Movies;

namespace SmartMovieCatalog.Domain.Tests.Movies;

public sealed class MovieTests
{
    [Fact]
    public void Create_WithValidValues_CreatesMovieWithNormalizedData()
    {
        DateTimeOffset createdAtUtc = new(2026, 5, 4, 12, 0, 0, TimeSpan.Zero);

        Movie movie = Movie.Create(
            MovieId.New(),
            " Central do Brasil ",
            " Central do Brasil ",
            1998,
            " br ",
            " pt-BR ",
            [MovieGenre.Create(" Drama "), MovieGenre.Create("Drama")],
            " Walter Salles ",
            " A retired teacher and a young boy travel through Brazil. ",
            110,
            " 12 ",
            createdAtUtc);

        Assert.NotEqual(Guid.Empty, movie.Id);
        Assert.Equal("Central do Brasil", movie.Title);
        Assert.Equal("Central do Brasil", movie.OriginalTitle);
        Assert.Equal(1998, movie.ReleaseYear);
        Assert.Equal("BR", movie.CountryCode);
        Assert.Equal("pt-BR", movie.OriginalLanguage);
        Assert.Single(movie.Genres);
        Assert.Contains(movie.Genres, genre => genre.Name == "Drama");
        Assert.Equal("Walter Salles", movie.Director);
        Assert.Equal("A retired teacher and a young boy travel through Brazil.", movie.Synopsis);
        Assert.Equal(110, movie.DurationMinutes);
        Assert.Equal("12", movie.AgeRating);
        Assert.Equal(createdAtUtc, movie.CreatedAtUtc);
    }

    [Fact]
    public void Create_WithOnlyRequiredValues_CreatesMovieWithEmptyOptionalData()
    {
        Movie movie = Movie.Create(
            MovieId.New(),
            "Central do Brasil",
            originalTitle: null,
            1998,
            "BR",
            "pt-BR",
            genres: null,
            director: null,
            synopsis: null,
            durationMinutes: null,
            ageRating: null,
            DateTimeOffset.UtcNow);

        Assert.Null(movie.OriginalTitle);
        Assert.Empty(movie.Genres);
        Assert.Null(movie.Director);
        Assert.Null(movie.Synopsis);
        Assert.Null(movie.DurationMinutes);
        Assert.Null(movie.AgeRating);
    }
}
