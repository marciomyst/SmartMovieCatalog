using SmartMovieCatalog.Domain.Movies;

namespace SmartMovieCatalog.Domain.Tests.Movies;

public sealed class GenreTests
{
    [Fact]
    public void Create_WithValidValues_NormalizesNameAndExternalId()
    {
        Genre genre = Genre.Create(GenreId.New(), " Drama ", 18);

        Assert.NotEqual(Guid.Empty, genre.Id);
        Assert.Equal("Drama", genre.Name);
        Assert.Equal("DRAMA", genre.NormalizedName);
        Assert.Equal(18, genre.ExternalId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_WithNonPositiveExternalId_Throws(int externalId)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Genre.Create(GenreId.New(), "Drama", externalId));
    }

    [Fact]
    public void From_WithEmptyGenreId_Throws()
    {
        Assert.Throws<ArgumentException>(() => GenreId.From(Guid.Empty));
    }

    [Fact]
    public void From_WithValidGenreId_ReturnsGenreId()
    {
        Guid id = Guid.NewGuid();

        GenreId genreId = GenreId.From(id);

        Assert.Equal(id, genreId.Value);
        Assert.Equal(id.ToString(), genreId.ToString());
    }
}
