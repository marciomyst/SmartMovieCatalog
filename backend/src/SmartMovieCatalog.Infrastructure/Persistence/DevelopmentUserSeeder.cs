using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using SmartMovieCatalog.Application.Abstractions.Authentication;
using SmartMovieCatalog.Domain.Users;

namespace SmartMovieCatalog.Infrastructure.Persistence;

public sealed class DevelopmentUserSeeder
{
    private readonly SmartMovieCatalogDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IConfiguration _configuration;

    public DevelopmentUserSeeder(
        SmartMovieCatalogDbContext dbContext,
        IPasswordHasher passwordHasher,
        IConfiguration configuration)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        string? email = _configuration["DevelopmentSeedUser:Email"];
        string? password = _configuration["DevelopmentSeedUser:Password"];
        string? name = _configuration["DevelopmentSeedUser:Name"];

        if (!EmailAddress.TryCreate(email, out EmailAddress? emailAddress) ||
            emailAddress is null ||
            string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        bool exists = await _dbContext.Users.AnyAsync(user => user.NormalizedEmail == emailAddress.NormalizedValue, cancellationToken);
        if (exists)
        {
            return;
        }

        string displayName = string.IsNullOrWhiteSpace(name) ? emailAddress.Value : name.Trim();
        User seedUser = User.Create(
            UserId.New(),
            emailAddress,
            displayName,
            "seed-pending",
            [UserRole.Create(UserRole.Admin)],
            mustChangePasswordOnFirstLogin: true,
            DateTimeOffset.UtcNow);

        seedUser.SetPasswordHash(_passwordHasher.HashPassword(seedUser, password), DateTimeOffset.UtcNow);

        await _dbContext.Users.AddAsync(seedUser, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
