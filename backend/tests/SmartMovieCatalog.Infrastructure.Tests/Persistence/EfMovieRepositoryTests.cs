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
        Genre genre = Genre.Create(GenreId.New(), " Drama ", 18);
        Movie movie = Movie.Create(
            MovieId.New(),
            " Central do Brasil ",
            " Central Station ",
            1998,
            " br ",
            " pt-BR ",
            [genre],
            " Walter Salles ",
            " Synopsis ",
            110,
            " 12 ",
            666,
            "/p/example-card.jpg",
            new DateTimeOffset(2026, 5, 4, 12, 0, 0, TimeSpan.Zero));

        await repository.AddAsync(movie, CancellationToken.None);
        await repository.SaveChangesAsync(CancellationToken.None);

        Movie persistedMovie = await dbContext.Movies
            .Include(savedMovie => savedMovie.Genres)
            .SingleAsync(savedMovie => savedMovie.Id == movie.Id);
        Assert.Equal("Central do Brasil", persistedMovie.Title);
        Assert.Equal("BR", persistedMovie.CountryCode);
        Assert.Equal(666, persistedMovie.ExternalId);
        Assert.Equal("/p/example-card.jpg", persistedMovie.Image);
        Assert.Contains(persistedMovie.Genres, movieGenre => movieGenre.Genre.Name == "Drama");
        Assert.Single(dbContext.Genres);
        Assert.Single(dbContext.Set<MovieGenre>());
    }

    [Fact]
    public async Task AddAsync_SavesTwoMoviesWithSharedGenre()
    {
        await using SmartMovieCatalogDbContext dbContext = CreateDbContext();
        EfMovieRepository repository = new(dbContext);
        Genre genre = Genre.Create(GenreId.New(), "Drama", externalId: null);
        Movie firstMovie = CreateMovie("Central do Brasil", genre);
        Movie secondMovie = CreateMovie("Ainda Estou Aqui", genre);

        await repository.AddAsync(firstMovie, CancellationToken.None);
        await repository.AddAsync(secondMovie, CancellationToken.None);
        await repository.SaveChangesAsync(CancellationToken.None);

        Assert.Single(dbContext.Genres);
        Assert.Equal(2, await dbContext.Set<MovieGenre>().CountAsync());
    }

    [Fact]
    public async Task ListAsync_WithQueryAndPaging_ReturnsMatchingMovies()
    {
        await using SmartMovieCatalogDbContext dbContext = CreateDbContext();
        EfMovieRepository repository = new(dbContext);
        Genre genre = Genre.Create(GenreId.New(), "Drama", externalId: null);
        Movie firstMovie = CreateMovie("Central do Brasil", genre);
        Movie secondMovie = CreateMovie("Ainda Estou Aqui", genre);
        await repository.AddAsync(firstMovie, CancellationToken.None);
        await repository.AddAsync(secondMovie, CancellationToken.None);
        await repository.SaveChangesAsync(CancellationToken.None);

        SmartMovieCatalog.Application.Abstractions.Persistence.PagedResult<Movie> result =
            await repository.ListAsync("central", 1, 12, CancellationToken.None);

        Movie movie = Assert.Single(result.Items);
        Assert.Equal(firstMovie.Id, movie.Id);
        Assert.Equal(1, result.TotalCount);
        Assert.Equal(1, result.TotalPages);
        Assert.Contains(movie.Genres, movieGenre => movieGenre.Genre.Name == "Drama");
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingMovie_ReturnsMovieWithGenres()
    {
        await using SmartMovieCatalogDbContext dbContext = CreateDbContext();
        EfMovieRepository repository = new(dbContext);
        Genre genre = Genre.Create(GenreId.New(), "Drama", externalId: null);
        Movie movie = CreateMovie("Central do Brasil", genre);
        await repository.AddAsync(movie, CancellationToken.None);
        await repository.SaveChangesAsync(CancellationToken.None);

        Movie? result = await repository.GetByIdAsync(movie.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(movie.Id, result.Id);
        Assert.Contains(result.Genres, movieGenre => movieGenre.Genre.Name == "Drama");
    }

    [Fact]
    public void Model_MapsGenreExternalIdAsUniqueAndMovieGenresAsAssociation()
    {
        using SmartMovieCatalogDbContext dbContext = CreateDbContext();

        Microsoft.EntityFrameworkCore.Metadata.IEntityType genreEntity = dbContext.Model.FindEntityType(typeof(Genre))!;
        Assert.Contains(
            genreEntity.GetIndexes(),
            index => index.IsUnique &&
                index.Properties.Count == 1 &&
                index.Properties.Single().Name == nameof(Genre.ExternalId));

        Microsoft.EntityFrameworkCore.Metadata.IEntityType movieGenreEntity = dbContext.Model.FindEntityType(typeof(MovieGenre))!;
        Assert.Null(movieGenreEntity.FindProperty("Name"));
        Assert.NotNull(movieGenreEntity.FindProperty(nameof(MovieGenre.MovieId)));
        Assert.NotNull(movieGenreEntity.FindProperty(nameof(MovieGenre.GenreId)));
    }

    private static Movie CreateMovie(string title, Genre genre)
    {
        return Movie.Create(
            MovieId.New(),
            title,
            originalTitle: null,
            2024,
            "BR",
            "pt-BR",
            [genre],
            director: null,
            synopsis: null,
            durationMinutes: null,
            ageRating: null,
            externalId: null,
            image: null,
            new DateTimeOffset(2026, 5, 4, 12, 0, 0, TimeSpan.Zero));
    }

    private static SmartMovieCatalogDbContext CreateDbContext()
    {
        DbContextOptions<SmartMovieCatalogDbContext> options = new DbContextOptionsBuilder<SmartMovieCatalogDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new SmartMovieCatalogDbContext(options);
    }
}
