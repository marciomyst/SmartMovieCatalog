using Microsoft.EntityFrameworkCore;
using SmartMovieCatalog.Domain.Users;
using SmartMovieCatalog.Infrastructure.Authentication;
using SmartMovieCatalog.Infrastructure.Persistence;

namespace SmartMovieCatalog.Infrastructure.Tests.Authentication;

public sealed class EfUserRepositoryTests
{
    [Fact]
    public async Task AddAsync_ThenFindByNormalizedEmailAsync_ReturnsUser()
    {
        using SmartMovieCatalogDbContext dbContext = CreateDbContext();
        EfUserRepository repository = new(dbContext);
        User user = CreateUser("user@example.com");

        await repository.AddAsync(user, CancellationToken.None);
        await repository.SaveChangesAsync(CancellationToken.None);

        User? found = await repository.FindByNormalizedEmailAsync(
            EmailAddress.Normalize("USER@example.com"),
            CancellationToken.None);

        Assert.NotNull(found);
        Assert.Equal(user.Id, found.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingUser_ReturnsUser()
    {
        using SmartMovieCatalogDbContext dbContext = CreateDbContext();
        User user = CreateUser("user@example.com");
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        EfUserRepository repository = new(dbContext);

        User? found = await repository.GetByIdAsync(UserId.From(user.Id), CancellationToken.None);

        Assert.NotNull(found);
        Assert.Equal(user.Email, found.Email);
    }

    [Fact]
    public async Task FindMethods_WithMissingUser_ReturnNull()
    {
        using SmartMovieCatalogDbContext dbContext = CreateDbContext();
        EfUserRepository repository = new(dbContext);

        User? byEmail = await repository.FindByNormalizedEmailAsync("MISSING@EXAMPLE.COM", CancellationToken.None);
        User? byId = await repository.GetByIdAsync(UserId.New(), CancellationToken.None);

        Assert.Null(byEmail);
        Assert.Null(byId);
    }

    private static SmartMovieCatalogDbContext CreateDbContext()
    {
        DbContextOptions<SmartMovieCatalogDbContext> options = new DbContextOptionsBuilder<SmartMovieCatalogDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new SmartMovieCatalogDbContext(options);
    }

    private static User CreateUser(string email)
    {
        return User.Create(
            UserId.New(),
            EmailAddress.Create(email),
            "Example User",
            "hash",
            [UserRole.Create(UserRole.User)],
            mustChangePasswordOnFirstLogin: false,
            DateTimeOffset.UtcNow);
    }
}
