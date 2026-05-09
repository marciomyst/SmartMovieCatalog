using SmartMovieCatalog.Application.Abstractions.Persistence;
using SmartMovieCatalog.Domain.Movies;

namespace SmartMovieCatalog.Application.Features.Movies;

public sealed class GetMovieByIdHandler
{
    private readonly IMovieRepository _movieRepository;

    public GetMovieByIdHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<Result<MovieDetails, GetMovieByIdFailure>> Handle(
        GetMovieByIdQuery query,
        CancellationToken cancellationToken)
    {
        Movie? movie = await _movieRepository.GetByIdAsync(query.Id, cancellationToken);
        if (movie is null)
        {
            return Result<MovieDetails, GetMovieByIdFailure>.Failure(GetMovieByIdFailure.NotFound);
        }

        return Result<MovieDetails, GetMovieByIdFailure>.Success(new MovieDetails(
            movie.Id,
            movie.Title,
            movie.OriginalTitle,
            movie.ReleaseYear,
            movie.CountryCode,
            movie.OriginalLanguage,
            movie.Genres
                .Select(movieGenre => movieGenre.Genre.Name)
                .Order(StringComparer.Ordinal)
                .ToArray(),
            movie.Director,
            movie.Synopsis,
            movie.DurationMinutes,
            movie.AgeRating,
            movie.ExternalId,
            movie.Image,
            movie.CreatedAtUtc));
    }
}
