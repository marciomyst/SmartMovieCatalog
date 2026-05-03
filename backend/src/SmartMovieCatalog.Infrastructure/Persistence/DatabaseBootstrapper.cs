using Microsoft.EntityFrameworkCore;

namespace SmartMovieCatalog.Infrastructure.Persistence;

public sealed class DatabaseBootstrapper
{
    private readonly SmartMovieCatalogDbContext _dbContext;
    private readonly AdminUserSeeder _adminUserSeeder;

    public DatabaseBootstrapper(
        SmartMovieCatalogDbContext dbContext,
        AdminUserSeeder adminUserSeeder)
    {
        _dbContext = dbContext;
        _adminUserSeeder = adminUserSeeder;
    }

    public async Task BootstrapAsync(CancellationToken cancellationToken)
    {
        await _dbContext.Database.MigrateAsync(cancellationToken);
        await _adminUserSeeder.SeedAsync(cancellationToken);
    }
}
