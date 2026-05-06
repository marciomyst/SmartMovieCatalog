using SmartMovieCatalog.Application.Abstractions.Persistence;
using SmartMovieCatalog.Application.Abstractions.Time;
using SmartMovieCatalog.Domain.Movies;

namespace SmartMovieCatalog.Application.Features.Movies;

public sealed class CreateMovieHandler
{
    private readonly IMovieRepository _movieRepository;
    private readonly IClock _clock;

    public CreateMovieHandler(IMovieRepository movieRepository, IClock clock)
    {
        _movieRepository = movieRepository;
        _clock = clock;
    }

    public async Task<CreatedMovie> Handle(CreateMovieCommand command, CancellationToken cancellationToken)
    {
        Movie movie = Movie.Create(
            MovieId.New(),
            command.Title,
            command.OriginalTitle,
            command.ReleaseYear,
            command.CountryCode,
            command.OriginalLanguage,
            command.Genres?.Select(MovieGenre.Create),
            command.Director,
            command.Synopsis,
            command.DurationMinutes,
            command.AgeRating,
            _clock.UtcNow);

        await _movieRepository.AddAsync(movie, cancellationToken);
        await _movieRepository.SaveChangesAsync(cancellationToken);

        return new CreatedMovie(
            movie.Id,
            movie.Title,
            movie.OriginalTitle,
            movie.ReleaseYear,
            movie.CountryCode,
            movie.OriginalLanguage,
            movie.Genres.Select(genre => genre.Name).ToArray(),
            movie.Director,
            movie.Synopsis,
            movie.DurationMinutes,
            movie.AgeRating);
    }
}
