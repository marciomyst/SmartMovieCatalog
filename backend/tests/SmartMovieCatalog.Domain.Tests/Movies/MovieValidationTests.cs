using SmartMovieCatalog.Domain.Movies;

namespace SmartMovieCatalog.Domain.Tests.Movies;

public sealed class MovieValidationTests
{
    [Fact]
    public void Create_WithEmptyMovieId_Throws()
    {
        Assert.Throws<ArgumentException>(() => MovieId.From(Guid.Empty));
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
    public void MovieGenre_WithBlankName_Throws()
    {
        Assert.Throws<ArgumentException>(() => MovieGenre.Create(" "));
    }

    private static Movie CreateMovie(
        string? title = "Central do Brasil",
        int releaseYear = 1998,
        string? countryCode = "BR",
        string? originalLanguage = "pt-BR",
        int? durationMinutes = null)
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
            DateTimeOffset.UtcNow);
    }
}
