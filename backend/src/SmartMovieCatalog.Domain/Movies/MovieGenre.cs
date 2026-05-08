using SmartMovieCatalog.Domain.Common;

namespace SmartMovieCatalog.Domain.Movies;

public sealed class MovieGenre : ValueObject
{
    private MovieGenre()
    {
        Genre = null!;
    }

    private MovieGenre(Guid movieId, Genre genre)
    {
        MovieId = movieId;
        GenreId = genre.Id;
        Genre = genre;
    }

    public Guid MovieId { get; private set; }

    public Guid GenreId { get; private set; }

    public Genre Genre { get; private set; }

    public static MovieGenre Create(Guid movieId, Genre genre)
    {
        ArgumentNullException.ThrowIfNull(genre);

        if (movieId == Guid.Empty)
        {
            throw new ArgumentException("Movie id cannot be empty.", nameof(movieId));
        }

        return new MovieGenre(movieId, genre);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return MovieId;
        yield return GenreId;
    }

    public override string ToString() => Genre.Name;
}
