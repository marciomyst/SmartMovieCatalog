using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartMovieCatalog.Application.Abstractions.Authentication;
using SmartMovieCatalog.Domain.Users;

namespace SmartMovieCatalog.Infrastructure.Persistence;

public sealed class AdminUserSeeder
{
    private readonly SmartMovieCatalogDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IConfiguration _configuration;

    public AdminUserSeeder(
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
        string? email = _configuration["AdminSeedUser:Email"];
        string? password = _configuration["AdminSeedUser:Password"];
        string? name = _configuration["AdminSeedUser:Name"];

        bool hasEmail = !string.IsNullOrWhiteSpace(email);
        bool hasPassword = !string.IsNullOrWhiteSpace(password);

        if (!hasEmail && !hasPassword)
        {
            return;
        }

        if (!hasEmail || !hasPassword)
        {
            throw new InvalidOperationException("AdminSeedUser:Email and AdminSeedUser:Password must be supplied together.");
        }

        if (!EmailAddress.TryCreate(email, out EmailAddress? emailAddress) || emailAddress is null)
        {
            throw new InvalidOperationException("AdminSeedUser:Email is not a valid email address.");
        }

        bool exists = await _dbContext.Users
            .AnyAsync(user => user.NormalizedEmail == emailAddress.NormalizedValue, cancellationToken);

        if (exists)
        {
            return;
        }

        string displayName = string.IsNullOrWhiteSpace(name) ? emailAddress.Value : name.Trim();
        User adminUser = User.Create(
            UserId.New(),
            emailAddress,
            displayName,
            "seed-pending",
            [UserRole.Create(UserRole.Admin)],
            mustChangePasswordOnFirstLogin: false,
            DateTimeOffset.UtcNow);

        adminUser.SetPasswordHash(_passwordHasher.HashPassword(adminUser, password!), DateTimeOffset.UtcNow);

        await _dbContext.Users.AddAsync(adminUser, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
