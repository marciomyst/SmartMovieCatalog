using SmartMovieCatalog.Domain.Movies;

namespace SmartMovieCatalog.Application.Abstractions.Persistence;

public interface IGenreRepository
{
    Task<IReadOnlyCollection<Genre>> FindByNormalizedNamesAsync(
        IReadOnlyCollection<string> normalizedNames,
        CancellationToken cancellationToken);

    Task AddRangeAsync(IReadOnlyCollection<Genre> genres, CancellationToken cancellationToken);
}
