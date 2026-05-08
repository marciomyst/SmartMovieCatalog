using Microsoft.EntityFrameworkCore;
using SmartMovieCatalog.Domain.Movies;
using SmartMovieCatalog.Domain.Users;

namespace SmartMovieCatalog.Infrastructure.Persistence;

public sealed class SmartMovieCatalogDbContext : DbContext
{
    public SmartMovieCatalogDbContext(DbContextOptions<SmartMovieCatalogDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Movie> Movies => Set<Movie>();

    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartMovieCatalogDbContext).Assembly);
    }
}
