using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartMovieCatalog.Domain.Users;
using SmartMovieCatalog.Infrastructure.Authentication;
using SmartMovieCatalog.Infrastructure.Persistence;

namespace SmartMovieCatalog.Infrastructure.Tests.Persistence;

public sealed class AdminUserSeederTests
{
    [Fact]
    public async Task SeedAsync_WithoutEmailAndPassword_DoesNotCreateUser()
    {
        using SmartMovieCatalogDbContext dbContext = CreateDbContext();
        AdminUserSeeder seeder = new(dbContext, new PasswordHasher(), CreateConfiguration([]));

        await seeder.SeedAsync(CancellationToken.None);

        Assert.Empty(dbContext.Users);
    }

    [Fact]
    public async Task SeedAsync_WithOnlyEmail_Throws()
    {
        using SmartMovieCatalogDbContext dbContext = CreateDbContext();
        AdminUserSeeder seeder = new(
            dbContext,
            new PasswordHasher(),
            CreateConfiguration(new Dictionary<string, string?>
            {
                ["AdminSeedUser:Email"] = "admin@example.com"
            }));

        await Assert.ThrowsAsync<InvalidOperationException>(() => seeder.SeedAsync(CancellationToken.None));
    }

    [Fact]
    public async Task SeedAsync_WithInvalidEmail_Throws()
    {
        using SmartMovieCatalogDbContext dbContext = CreateDbContext();
        AdminUserSeeder seeder = new(
            dbContext,
            new PasswordHasher(),
            CreateConfiguration(new Dictionary<string, string?>
            {
                ["AdminSeedUser:Email"] = "not-an-email",
                ["AdminSeedUser:Password"] = "Password123!"
            }));

        await Assert.ThrowsAsync<InvalidOperationException>(() => seeder.SeedAsync(CancellationToken.None));
    }

    [Fact]
    public async Task SeedAsync_WithValidConfiguration_CreatesAdminUser()
    {
        using SmartMovieCatalogDbContext dbContext = CreateDbContext();
        PasswordHasher passwordHasher = new();
        AdminUserSeeder seeder = new(
            dbContext,
            passwordHasher,
            CreateConfiguration(new Dictionary<string, string?>
            {
                ["AdminSeedUser:Email"] = " admin@example.com ",
                ["AdminSeedUser:Password"] = "Password123!",
                ["AdminSeedUser:Name"] = " Admin "
            }));

        await seeder.SeedAsync(CancellationToken.None);

        User admin = Assert.Single(dbContext.Users.Include(user => user.Roles));
        Assert.Equal("admin@example.com", admin.Email.Value);
        Assert.Equal("Admin", admin.Name);
        Assert.Contains(admin.Roles, role => role.Name == UserRole.Admin);
        Assert.True(passwordHasher.VerifyPassword(admin, "Password123!"));
    }

    [Fact]
    public async Task SeedAsync_WithExistingAdmin_DoesNotCreateDuplicate()
    {
        using SmartMovieCatalogDbContext dbContext = CreateDbContext();
        PasswordHasher passwordHasher = new();
        IConfiguration configuration = CreateConfiguration(new Dictionary<string, string?>
        {
            ["AdminSeedUser:Email"] = "admin@example.com",
            ["AdminSeedUser:Password"] = "Password123!"
        });
        AdminUserSeeder seeder = new(dbContext, passwordHasher, configuration);

        await seeder.SeedAsync(CancellationToken.None);
        await seeder.SeedAsync(CancellationToken.None);

        Assert.Single(dbContext.Users);
    }

    private static SmartMovieCatalogDbContext CreateDbContext()
    {
        DbContextOptions<SmartMovieCatalogDbContext> options = new DbContextOptionsBuilder<SmartMovieCatalogDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new SmartMovieCatalogDbContext(options);
    }

    private static IConfiguration CreateConfiguration(Dictionary<string, string?> values)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();
    }
}
