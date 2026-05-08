using SmartMovieCatalog.Application.Abstractions.Persistence;
using SmartMovieCatalog.Application.Abstractions.Time;
using SmartMovieCatalog.Domain.Movies;

namespace SmartMovieCatalog.Application.Features.Movies;

public sealed class CreateMovieHandler
{
    private readonly IMovieRepository _movieRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IClock _clock;

    public CreateMovieHandler(
        IMovieRepository movieRepository,
        IGenreRepository genreRepository,
        IClock clock)
    {
        _movieRepository = movieRepository;
        _genreRepository = genreRepository;
        _clock = clock;
    }

    public async Task<CreatedMovie> Handle(CreateMovieCommand command, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<Genre> genres = await ResolveGenresAsync(command.Genres, cancellationToken);

        Movie movie = Movie.Create(
            MovieId.New(),
            command.Title,
            command.OriginalTitle,
            command.ReleaseYear,
            command.CountryCode,
            command.OriginalLanguage,
            genres,
            command.Director,
            command.Synopsis,
            command.DurationMinutes,
            command.AgeRating,
            command.ExternalId,
            command.Image,
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
            movie.Genres.Select(movieGenre => movieGenre.Genre.Name).ToArray(),
            movie.Director,
            movie.Synopsis,
            movie.DurationMinutes,
            movie.AgeRating,
            movie.ExternalId,
            movie.Image);
    }

    private async Task<IReadOnlyCollection<Genre>> ResolveGenresAsync(
        IReadOnlyCollection<string>? genreNames,
        CancellationToken cancellationToken)
    {
        if (genreNames is null || genreNames.Count == 0)
        {
            return [];
        }

        Dictionary<string, string> requestedGenres = new(StringComparer.Ordinal);
        foreach (string genreName in genreNames)
        {
            string normalizedName = Genre.NormalizeName(genreName);
            requestedGenres.TryAdd(normalizedName, genreName.Trim());
        }

        if (requestedGenres.Count == 0)
        {
            return [];
        }

        string[] normalizedNames = requestedGenres.Keys.ToArray();
        IReadOnlyCollection<Genre> existingGenres = await _genreRepository.FindByNormalizedNamesAsync(
            normalizedNames,
            cancellationToken);

        HashSet<string> existingNormalizedNames = existingGenres
            .Select(genre => genre.NormalizedName)
            .ToHashSet(StringComparer.Ordinal);

        Genre[] newGenres = requestedGenres
            .Where(requestedGenre => !existingNormalizedNames.Contains(requestedGenre.Key))
            .Select(requestedGenre => Genre.Create(GenreId.New(), requestedGenre.Value, externalId: null))
            .ToArray();

        if (newGenres.Length > 0)
        {
            await _genreRepository.AddRangeAsync(newGenres, cancellationToken);
        }

        Dictionary<string, Genre> genresByNormalizedName = existingGenres
            .Concat(newGenres)
            .ToDictionary(genre => genre.NormalizedName, StringComparer.Ordinal);

        return requestedGenres.Keys
            .Select(normalizedName => genresByNormalizedName[normalizedName])
            .ToArray();
    }
}
