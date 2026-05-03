using Microsoft.EntityFrameworkCore;
using SmartMovieCatalog.Application.Abstractions.Persistence;
using SmartMovieCatalog.Domain.Users;
using SmartMovieCatalog.Infrastructure.Persistence;

namespace SmartMovieCatalog.Infrastructure.Authentication;

public sealed class EfUserRepository : IUserRepository
{
    private readonly SmartMovieCatalogDbContext _dbContext;

    public EfUserRepository(SmartMovieCatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<User?> FindByNormalizedEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        return _dbContext.Users
            .SingleOrDefaultAsync(user => user.NormalizedEmail == normalizedEmail, cancellationToken);
    }

    public Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken)
    {
        return _dbContext.Users
            .SingleOrDefaultAsync(user => user.Id == id.Value, cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
