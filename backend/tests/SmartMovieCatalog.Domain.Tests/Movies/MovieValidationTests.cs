using SmartMovieCatalog.Domain.Movies;

namespace SmartMovieCatalog.Domain.Tests.Movies;

public sealed class MovieValidationTests
{
    [Fact]
    public void Create_WithEmptyMovieId_Throws()
    {
        Assert.Throws<ArgumentException>(() => MovieId.From(Guid.Empty));
    }

    [Fact]
    public void Create_WithValidMovieId_ReturnsMovieId()
    {
        Guid id = Guid.NewGuid();

        MovieId movieId = MovieId.From(id);

        Assert.Equal(id, movieId.Value);
        Assert.Equal(id.ToString(), movieId.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithBlankTitle_Throws(string? title)
    {
        Assert.Throws<ArgumentException>(() => CreateMovie(title: title));
    }

    [Fact]
    public void Create_WithReleaseYearBeforeFirstFilmYear_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => CreateMovie(releaseYear: 1887));
    }

    [Fact]
    public void Create_WithReleaseYearAfterNextCalendarYear_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => CreateMovie(releaseYear: DateTimeOffset.UtcNow.Year + 2));
    }

    [Theory]
    [InlineData("B")]
    [InlineData("BRA")]
    [InlineData("B1")]
    public void Create_WithInvalidCountryCode_Throws(string countryCode)
    {
        Assert.Throws<ArgumentException>(() => CreateMovie(countryCode: countryCode));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithBlankOriginalLanguage_Throws(string? originalLanguage)
    {
        Assert.Throws<ArgumentException>(() => CreateMovie(originalLanguage: originalLanguage));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_WithNonPositiveDuration_Throws(int durationMinutes)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => CreateMovie(durationMinutes: durationMinutes));
    }

    [Fact]
    public void Create_WithExternalIdAndImage_NormalizesValues()
    {
        Movie movie = CreateMovie(externalId: 666, image: " /p/example-card.jpg ");

        Assert.Equal(666, movie.ExternalId);
        Assert.Equal("/p/example-card.jpg", movie.Image);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_WithNonPositiveExternalId_Throws(int externalId)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => CreateMovie(externalId: externalId));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithBlankImage_NormalizesToNull(string? image)
    {
        Movie movie = CreateMovie(image: image);

        Assert.Null(movie.Image);
    }

    [Fact]
    public void Create_WithoutExternalIdAndImage_KeepsValuesNull()
    {
        Movie movie = CreateMovie();

        Assert.Null(movie.ExternalId);
        Assert.Null(movie.Image);
    }

    [Theory]
    [InlineData("https://image.tmdb.org/t/p/w500/example.jpg")]
    [InlineData("//image.tmdb.org/t/p/w500/example.jpg")]
    [InlineData("p/example-card.jpg")]
    [InlineData("/p\\example-card.jpg")]
    public void Create_WithNonRelativeImage_Throws(string image)
    {
        Assert.Throws<ArgumentException>(() => CreateMovie(image: image));
    }

    [Fact]
    public void Create_WithRelativeImagePath_KeepsNormalizedPath()
    {
        Movie movie = CreateMovie(image: "/p/example-card.jpg");

        Assert.Equal("/p/example-card.jpg", movie.Image);
    }

    [Fact]
    public void Create_WithLowercaseCountryCode_NormalizesToUppercase()
    {
        Movie movie = CreateMovie(countryCode: "br");

        Assert.Equal("BR", movie.CountryCode);
    }

    [Fact]
    public void Genre_WithBlankName_Throws()
    {
        Assert.Throws<ArgumentException>(() => Genre.Create(GenreId.New(), " ", externalId: null));
    }

    [Fact]
    public void MovieGenre_WithEmptyMovieId_Throws()
    {
        Genre genre = Genre.Create(GenreId.New(), "Drama", externalId: null);

        Assert.Throws<ArgumentException>(() => MovieGenre.Create(Guid.Empty, genre));
    }

    [Fact]
    public void MovieGenre_WithNullGenre_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => MovieGenre.Create(Guid.NewGuid(), genre: null!));
    }

    [Fact]
    public void MovieGenre_ToString_ReturnsGenreName()
    {
        Genre genre = Genre.Create(GenreId.New(), "Drama", externalId: null);

        MovieGenre movieGenre = MovieGenre.Create(Guid.NewGuid(), genre);

        Assert.Equal("Drama", movieGenre.ToString());
    }

    private static Movie CreateMovie(
        string? title = "Central do Brasil",
        int releaseYear = 1998,
        string? countryCode = "BR",
        string? originalLanguage = "pt-BR",
        int? durationMinutes = null,
        int? externalId = null,
        string? image = null)
    {
        return Movie.Create(
            MovieId.New(),
            title!,
            originalTitle: null,
            releaseYear,
            countryCode!,
            originalLanguage!,
            genres: null,
            director: null,
            synopsis: null,
            durationMinutes,
            ageRating: null,
            externalId,
            image,
            DateTimeOffset.UtcNow);
    }
}
