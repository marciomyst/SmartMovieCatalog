using Microsoft.EntityFrameworkCore;
using SmartMovieCatalog.Domain.Movies;
using SmartMovieCatalog.Infrastructure.Persistence;

namespace SmartMovieCatalog.Infrastructure.Tests.Persistence;

public sealed class EfGenreRepositoryTests
{
    [Fact]
    public async Task FindByNormalizedNamesAsync_WithEmptyInput_ReturnsEmpty()
    {
        await using SmartMovieCatalogDbContext dbContext = CreateDbContext();
        EfGenreRepository repository = new(dbContext);

        IReadOnlyCollection<Genre> genres = await repository.FindByNormalizedNamesAsync(
            [],
            CancellationToken.None);

        Assert.Empty(genres);
    }

    [Fact]
    public async Task AddRangeAsync_WithEmptyInput_DoesNotChangeDatabase()
    {
        await using SmartMovieCatalogDbContext dbContext = CreateDbContext();
        EfGenreRepository repository = new(dbContext);

        await repository.AddRangeAsync([], CancellationToken.None);
        await dbContext.SaveChangesAsync();

        Assert.Empty(dbContext.Genres);
    }

    [Fact]
    public async Task FindByNormalizedNamesAsync_ReturnsMatchingGenresOnly()
    {
        await using SmartMovieCatalogDbContext dbContext = CreateDbContext();
        EfGenreRepository repository = new(dbContext);
        Genre drama = Genre.Create(GenreId.New(), "Drama", 18);
        Genre comedy = Genre.Create(GenreId.New(), "Comedy", 35);

        await repository.AddRangeAsync([drama, comedy], CancellationToken.None);
        await dbContext.SaveChangesAsync();

        IReadOnlyCollection<Genre> genres = await repository.FindByNormalizedNamesAsync(
            [Genre.NormalizeName(" drama "), Genre.NormalizeName("Action")],
            CancellationToken.None);

        Genre genre = Assert.Single(genres);
        Assert.Equal(drama.Id, genre.Id);
        Assert.Equal("Drama", genre.Name);
        Assert.Equal(18, genre.ExternalId);
    }

    private static SmartMovieCatalogDbContext CreateDbContext()
    {
        DbContextOptions<SmartMovieCatalogDbContext> options = new DbContextOptionsBuilder<SmartMovieCatalogDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new SmartMovieCatalogDbContext(options);
    }
}
