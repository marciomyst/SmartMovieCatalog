using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartMovieCatalog.Domain.Movies;

namespace SmartMovieCatalog.Infrastructure.Persistence.Configurations;

public sealed class MovieGenreConfiguration : IEntityTypeConfiguration<MovieGenre>
{
    public void Configure(EntityTypeBuilder<MovieGenre> builder)
    {
        builder.ToTable("MovieGenres");

        builder.HasKey(movieGenre => new { movieGenre.MovieId, movieGenre.GenreId });

        builder.Property(movieGenre => movieGenre.MovieId)
            .IsRequired();

        builder.Property(movieGenre => movieGenre.GenreId)
            .IsRequired();

        builder.HasOne<Movie>()
            .WithMany(movie => movie.Genres)
            .HasForeignKey(movieGenre => movieGenre.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(movieGenre => movieGenre.Genre)
            .WithMany()
            .HasForeignKey(movieGenre => movieGenre.GenreId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
