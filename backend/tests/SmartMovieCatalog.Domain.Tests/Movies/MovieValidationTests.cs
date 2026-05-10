using SmartMovieCatalog.Domain.Movies;

namespace SmartMovieCatalog.Domain.Tests.Movies;

public sealed class MovieValidationTests
{
    [Fact]
    public void Create_WithEmptyMovieId_Throws()
    {
        ArgumentException exception = Assert.Throws<ArgumentException>(() => MovieId.From(Guid.Empty));

        Assert.Equal("value", exception.ParamName);
        Assert.Contains("Movie id cannot be empty.", exception.Message);
    }

    [Fact]
    public void Create_WithValidMovieId_ReturnsMovieId()
    {
        Guid id = Guid.NewGuid();

        MovieId movieId = MovieId.From(id);

        Assert.Equal(id, movieId.Value);
        Assert.Equal(id.ToString(), movieId.ToString());
    }

    [Fact]
    public void Create_WithBoundaryReleaseYearValues_AcceptsLowerAndUpperBounds()
    {
        int upperBoundReleaseYear = DateTimeOffset.UtcNow.Year + 1;

        Movie lowerBoundMovie = CreateMovie(releaseYear: 1888);
        Movie upperBoundMovie = CreateMovie(releaseYear: upperBoundReleaseYear);

        Assert.Equal(1888, lowerBoundMovie.ReleaseYear);
        Assert.Equal(upperBoundReleaseYear, upperBoundMovie.ReleaseYear);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithBlankTitle_Throws(string? title)
    {
        ArgumentException exception = Assert.Throws<ArgumentException>(() => CreateMovie(title: title));

        Assert.Equal("title", exception.ParamName);
        Assert.Contains("null or whitespace", exception.Message);
    }

    [Fact]
    public void Create_WithReleaseYearBeforeFirstFilmYear_Throws()
    {
        ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() => CreateMovie(releaseYear: 1887));

        Assert.Equal("releaseYear", exception.ParamName);
        Assert.Contains("between 1888 and", exception.Message);
    }

    [Fact]
    public void Create_WithReleaseYearAfterNextCalendarYear_Throws()
    {
        ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() => CreateMovie(releaseYear: DateTimeOffset.UtcNow.Year + 2));

        Assert.Equal("releaseYear", exception.ParamName);
        Assert.Contains("between 1888 and", exception.Message);
    }

    [Theory]
    [InlineData("B")]
    [InlineData("BRA")]
    [InlineData("B1")]
    public void Create_WithInvalidCountryCode_Throws(string countryCode)
    {
        ArgumentException exception = Assert.Throws<ArgumentException>(() => CreateMovie(countryCode: countryCode));

        Assert.Equal("countryCode", exception.ParamName);
        Assert.Contains("Country code must contain exactly two letters.", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithBlankOriginalLanguage_Throws(string? originalLanguage)
    {
        ArgumentException exception = Assert.Throws<ArgumentException>(() => CreateMovie(originalLanguage: originalLanguage));

        Assert.Equal("originalLanguage", exception.ParamName);
        Assert.Contains("null or whitespace", exception.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_WithNonPositiveDuration_Throws(int durationMinutes)
    {
        ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() => CreateMovie(durationMinutes: durationMinutes));

        Assert.Equal("durationMinutes", exception.ParamName);
        Assert.Contains("Duration must be positive when supplied.", exception.Message);
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
        ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() => CreateMovie(externalId: externalId));

        Assert.Equal("externalId", exception.ParamName);
        Assert.Contains("External ID must be positive when supplied.", exception.Message);
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
        ArgumentException exception = Assert.Throws<ArgumentException>(() => CreateMovie(image: image));

        Assert.Equal("image", exception.ParamName);
        Assert.Contains("Image must be a relative path.", exception.Message);
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
        ArgumentException exception = Assert.Throws<ArgumentException>(() => Genre.Create(GenreId.New(), " ", externalId: null));

        Assert.Equal("name", exception.ParamName);
        Assert.Contains("null or whitespace", exception.Message);
    }

    [Fact]
    public void MovieGenre_WithEmptyMovieId_Throws()
    {
        Genre genre = Genre.Create(GenreId.New(), "Drama", externalId: null);

        ArgumentException exception = Assert.Throws<ArgumentException>(() => MovieGenre.Create(Guid.Empty, genre));

        Assert.Equal("movieId", exception.ParamName);
        Assert.Contains("Movie id cannot be empty.", exception.Message);
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
