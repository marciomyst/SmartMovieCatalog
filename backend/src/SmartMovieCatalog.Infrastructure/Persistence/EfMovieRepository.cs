using SmartMovieCatalog.Application.Abstractions.Persistence;
using SmartMovieCatalog.Domain.Movies;

namespace SmartMovieCatalog.Infrastructure.Persistence;

public sealed class EfMovieRepository : IMovieRepository
{
    private readonly SmartMovieCatalogDbContext _dbContext;

    public EfMovieRepository(SmartMovieCatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Movie movie, CancellationToken cancellationToken)
    {
        await _dbContext.Movies.AddAsync(movie, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
