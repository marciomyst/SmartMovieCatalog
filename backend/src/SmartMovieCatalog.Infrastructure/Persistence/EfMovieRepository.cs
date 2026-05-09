using Microsoft.EntityFrameworkCore;
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

    public Task<Movie?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Movies
            .AsNoTracking()
            .Include(movie => movie.Genres)
            .ThenInclude(movieGenre => movieGenre.Genre)
            .SingleOrDefaultAsync(movie => movie.Id == id, cancellationToken);
    }

    public async Task<PagedResult<Movie>> ListAsync(
        string? query,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        IQueryable<Movie> movies = _dbContext.Movies.AsNoTracking();
        string? normalizedQuery = NormalizeQuery(query);

        if (normalizedQuery is not null)
        {
            string upperQuery = normalizedQuery.ToUpperInvariant();
            movies = movies.Where(movie =>
                movie.Title.ToUpper().Contains(upperQuery) ||
                (movie.OriginalTitle != null && movie.OriginalTitle.ToUpper().Contains(upperQuery)));
        }

        int totalCount = await movies.CountAsync(cancellationToken);
        Movie[] items = await movies
            .Include(movie => movie.Genres)
            .ThenInclude(movieGenre => movieGenre.Genre)
            .OrderByDescending(movie => movie.CreatedAtUtc)
            .ThenBy(movie => movie.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken);

        return new PagedResult<Movie>(items, page, pageSize, totalCount);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static string? NormalizeQuery(string? query)
    {
        string? trimmedQuery = query?.Trim();

        return string.IsNullOrEmpty(trimmedQuery) ? null : trimmedQuery;
    }
}
