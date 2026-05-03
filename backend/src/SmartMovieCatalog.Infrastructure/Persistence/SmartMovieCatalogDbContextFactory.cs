using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SmartMovieCatalog.Infrastructure.Persistence;

public sealed class SmartMovieCatalogDbContextFactory : IDesignTimeDbContextFactory<SmartMovieCatalogDbContext>
{
    public SmartMovieCatalogDbContext CreateDbContext(string[] args)
    {
        string connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? "Host=localhost;Port=5432;Database=smart_movie_catalog;Username=smartmovie;Password=smartmovie_dev_password";

        DbContextOptionsBuilder<SmartMovieCatalogDbContext> optionsBuilder = new();
        optionsBuilder.UseNpgsql(connectionString);

        return new SmartMovieCatalogDbContext(optionsBuilder.Options);
    }
}
