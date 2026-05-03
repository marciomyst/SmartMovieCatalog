using SmartMovieCatalog.Application.Abstractions.Persistence;
using SmartMovieCatalog.Domain.Users;

namespace SmartMovieCatalog.Api.Tests.TestSupport;

public sealed class FakeUserRepository : IUserRepository
{
    private readonly Dictionary<Guid, User> _users = [];

    public Task<User?> FindByNormalizedEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        User? user = _users.Values.SingleOrDefault(candidate => candidate.NormalizedEmail == normalizedEmail);
        return Task.FromResult(user);
    }

    public Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken)
    {
        _users.TryGetValue(id.Value, out User? user);
        return Task.FromResult(user);
    }

    public Task AddAsync(User user, CancellationToken cancellationToken)
    {
        _users[user.Id] = user;
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void Add(User user)
    {
        _users[user.Id] = user;
    }

    public void Remove(User user)
    {
        _users.Remove(user.Id);
    }

    public void Clear()
    {
        _users.Clear();
    }
}
