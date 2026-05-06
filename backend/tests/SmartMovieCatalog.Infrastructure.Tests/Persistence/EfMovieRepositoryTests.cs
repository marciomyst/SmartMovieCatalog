using Microsoft.EntityFrameworkCore;
using SmartMovieCatalog.Domain.Movies;
using SmartMovieCatalog.Infrastructure.Persistence;

namespace SmartMovieCatalog.Infrastructure.Tests.Persistence;

public sealed class EfMovieRepositoryTests
{
    [Fact]
    public async Task AddAsync_SavesMovieWithGenres()
    {
        await using SmartMovieCatalogDbContext dbContext = CreateDbContext();
        EfMovieRepository repository = new(dbContext);
        Movie movie = Movie.Create(
            MovieId.New(),
            " Central do Brasil ",
            " Central Station ",
            1998,
            " br ",
            " pt-BR ",
            [MovieGenre.Create(" Drama ")],
            " Walter Salles ",
            " Synopsis ",
            110,
            " 12 ",
            new DateTimeOffset(2026, 5, 4, 12, 0, 0, TimeSpan.Zero));

        await repository.AddAsync(movie, CancellationToken.None);
        await repository.SaveChangesAsync(CancellationToken.None);

        Movie persistedMovie = await dbContext.Movies
            .Include(savedMovie => savedMovie.Genres)
            .SingleAsync(savedMovie => savedMovie.Id == movie.Id);
        Assert.Equal("Central do Brasil", persistedMovie.Title);
        Assert.Equal("BR", persistedMovie.CountryCode);
        Assert.Contains(persistedMovie.Genres, genre => genre.Name == "Drama");
    }

    private static SmartMovieCatalogDbContext CreateDbContext()
    {
        DbContextOptions<SmartMovieCatalogDbContext> options = new DbContextOptionsBuilder<SmartMovieCatalogDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new SmartMovieCatalogDbContext(options);
    }
}
