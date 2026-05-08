using Microsoft.EntityFrameworkCore;
using SmartMovieCatalog.Application.Abstractions.Persistence;
using SmartMovieCatalog.Domain.Movies;

namespace SmartMovieCatalog.Infrastructure.Persistence;

public sealed class EfGenreRepository : IGenreRepository
{
    private readonly SmartMovieCatalogDbContext _dbContext;

    public EfGenreRepository(SmartMovieCatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Genre>> FindByNormalizedNamesAsync(
        IReadOnlyCollection<string> normalizedNames,
        CancellationToken cancellationToken)
    {
        if (normalizedNames.Count == 0)
        {
            return [];
        }

        return await _dbContext.Genres
            .Where(genre => normalizedNames.Contains(genre.NormalizedName))
            .ToArrayAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IReadOnlyCollection<Genre> genres, CancellationToken cancellationToken)
    {
        if (genres.Count == 0)
        {
            return;
        }

        await _dbContext.Genres.AddRangeAsync(genres, cancellationToken);
    }
}
