using SmartMovieCatalog.Domain.Movies;

namespace SmartMovieCatalog.Domain.Tests.Movies;

public sealed class MovieGenreTests
{
    [Fact]
    public void Create_WithValidMovieIdAndGenre_ReturnsMovieGenre()
    {
        Guid movieId = Guid.NewGuid();
        Genre genre = Genre.Create(GenreId.New(), "Drama", null);

        MovieGenre movieGenre = MovieGenre.Create(movieId, genre);

        Assert.Equal(movieId, movieGenre.MovieId);
        Assert.Equal(genre.Id, movieGenre.GenreId);
        Assert.Equal(genre, movieGenre.Genre);
        Assert.Equal("Drama", movieGenre.ToString());
    }

    [Fact]
    public void Create_WithEmptyMovieId_ThrowsArgumentException()
    {
        Genre genre = Genre.Create(GenreId.New(), "Drama", null);

        Assert.Throws<ArgumentException>(() => MovieGenre.Create(Guid.Empty, genre));
    }

    [Fact]
    public void Create_WithNullGenre_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => MovieGenre.Create(Guid.NewGuid(), null!));
    }

    [Fact]
    public void Equality_IsBasedOnMovieIdAndGenreId()
    {
        Guid movieId = Guid.NewGuid();
        Genre genre1 = Genre.Create(GenreId.New(), "Drama", null);
        Genre genre2 = Genre.Create(GenreId.New(), "Drama", null);

        MovieGenre movieGenre1 = MovieGenre.Create(movieId, genre1);
        MovieGenre movieGenre2 = MovieGenre.Create(movieId, genre1);
        MovieGenre movieGenre3 = MovieGenre.Create(movieId, genre2);

        Assert.Equal(movieGenre1, movieGenre2);
        Assert.NotEqual(movieGenre1, movieGenre3);
    }

    [Fact]
    public void ToString_ReturnsGenreName()
    {
        Guid movieId = Guid.NewGuid();
        Genre genre = Genre.Create(GenreId.New(), "Comedy", null);

        MovieGenre movieGenre = MovieGenre.Create(movieId, genre);

        Assert.Equal("Comedy", movieGenre.ToString());
    }
}