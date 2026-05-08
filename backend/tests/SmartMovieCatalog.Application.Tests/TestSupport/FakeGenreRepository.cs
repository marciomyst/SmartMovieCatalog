using SmartMovieCatalog.Application.Abstractions.Persistence;
using SmartMovieCatalog.Domain.Movies;

namespace SmartMovieCatalog.Application.Tests.TestSupport;

public sealed class FakeGenreRepository : IGenreRepository
{
    private readonly List<Genre> _genres = [];

    public IReadOnlyCollection<Genre> Genres => _genres.AsReadOnly();

    public void Add(Genre genre)
    {
        _genres.Add(genre);
    }

    public Task<IReadOnlyCollection<Genre>> FindByNormalizedNamesAsync(
        IReadOnlyCollection<string> normalizedNames,
        CancellationToken cancellationToken)
    {
        IReadOnlyCollection<Genre> genres = _genres
            .Where(genre => normalizedNames.Contains(genre.NormalizedName))
            .ToArray();

        return Task.FromResult(genres);
    }

    public Task AddRangeAsync(IReadOnlyCollection<Genre> genres, CancellationToken cancellationToken)
    {
        _genres.AddRange(genres);

        return Task.CompletedTask;
    }
}
