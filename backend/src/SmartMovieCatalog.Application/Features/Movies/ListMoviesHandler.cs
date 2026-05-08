using SmartMovieCatalog.Application.Abstractions.Persistence;
using SmartMovieCatalog.Domain.Movies;

namespace SmartMovieCatalog.Application.Features.Movies;

public sealed class ListMoviesHandler
{
    private readonly IMovieRepository _movieRepository;

    public ListMoviesHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<PagedMovieSummaries> Handle(ListMoviesQuery query, CancellationToken cancellationToken)
    {
        PagedResult<Movie> movies = await _movieRepository.ListAsync(
            query.Query,
            query.Page,
            query.PageSize,
            cancellationToken);

        MovieSummary[] summaries = movies.Items
            .Select(movie => new MovieSummary(
                movie.Id,
                movie.Title,
                movie.ReleaseYear,
                movie.CountryCode,
                movie.Genres
                    .Select(movieGenre => movieGenre.Genre.Name)
                    .Order(StringComparer.Ordinal)
                    .ToArray(),
                movie.Director,
                movie.Image,
                movie.CreatedAtUtc))
            .ToArray();

        return new PagedMovieSummaries(
            summaries,
            movies.Page,
            movies.PageSize,
            movies.TotalCount,
            movies.TotalPages,
            movies.HasPreviousPage,
            movies.HasNextPage);
    }
}
