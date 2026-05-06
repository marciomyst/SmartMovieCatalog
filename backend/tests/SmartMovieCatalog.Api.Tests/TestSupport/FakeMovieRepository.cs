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

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
