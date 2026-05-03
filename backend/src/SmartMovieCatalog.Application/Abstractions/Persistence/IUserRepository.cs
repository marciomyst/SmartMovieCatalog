using SmartMovieCatalog.Domain.Users;

namespace SmartMovieCatalog.Application.Abstractions.Persistence;

public interface IUserRepository
{
    Task<User?> FindByNormalizedEmailAsync(string normalizedEmail, CancellationToken cancellationToken);

    Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken);

    Task AddAsync(User user, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
