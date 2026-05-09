using SmartMovieCatalog.Domain.Movies;

namespace SmartMovieCatalog.Application.Abstractions.Persistence;

public interface IMovieRepository
{
    Task AddAsync(Movie movie, CancellationToken cancellationToken);

    Task<Movie?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<PagedResult<Movie>> ListAsync(
        string? query,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
