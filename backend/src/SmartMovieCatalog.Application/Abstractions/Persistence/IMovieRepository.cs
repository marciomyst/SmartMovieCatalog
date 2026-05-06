using SmartMovieCatalog.Domain.Movies;

namespace SmartMovieCatalog.Application.Abstractions.Persistence;

public interface IMovieRepository
{
    Task AddAsync(Movie movie, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
