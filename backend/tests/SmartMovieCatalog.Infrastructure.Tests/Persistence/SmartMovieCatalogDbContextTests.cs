using Microsoft.EntityFrameworkCore;
using SmartMovieCatalog.Infrastructure.Persistence;

namespace SmartMovieCatalog.Infrastructure.Tests.Persistence;

public sealed class SmartMovieCatalogDbContextTests
{
    [Fact]
    public void Model_CanBeBuiltWithoutDatabaseConnection()
    {
        DbContextOptions<SmartMovieCatalogDbContext> options = new DbContextOptionsBuilder<SmartMovieCatalogDbContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=smart_movie_catalog_tests;Username=smartmovie;Password=placeholder")
            .Options;

        using SmartMovieCatalogDbContext dbContext = new(options);

        Assert.NotNull(dbContext.Model.FindEntityType("SmartMovieCatalog.Domain.Users.User"));
    }
}
