using SmartMovieCatalog.Application.Abstractions.Persistence;
using SmartMovieCatalog.Domain.Movies;

namespace SmartMovieCatalog.Api.Tests.TestSupport;

public sealed class FakeMovieRepository : IMovieRepository
{
    private readonly List<Movie> _movies = [];

    public IReadOnlyCollection<Movie> Movies => _movies.AsReadOnly();

    public void Clear()
    {
        _movies.Clear();
    }

    public Task AddAsync(Movie movie, CancellationToken cancellationToken)
    {
        _movies.Add(movie);

        return Task.CompletedTask;
    }

    public Task<PagedResult<Movie>> ListAsync(
        string? query,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        IEnumerable<Movie> movies = _movies;
        string? normalizedQuery = NormalizeQuery(query);

        if (normalizedQuery is not null)
        {
            movies = movies.Where(movie =>
                movie.Title.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase) ||
                (movie.OriginalTitle?.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase) ?? false));
        }

        Movie[] matchingMovies = movies.ToArray();
        Movie[] items = matchingMovies
            .OrderByDescending(movie => movie.CreatedAtUtc)
            .ThenBy(movie => movie.Title, StringComparer.Ordinal)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToArray();

        return Task.FromResult(new PagedResult<Movie>(items, page, pageSize, matchingMovies.Length));
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private static string? NormalizeQuery(string? query)
    {
        string? trimmedQuery = query?.Trim();

        return string.IsNullOrEmpty(trimmedQuery) ? null : trimmedQuery;
    }
}
