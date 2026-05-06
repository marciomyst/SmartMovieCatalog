using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartMovieCatalog.Domain.Movies;

namespace SmartMovieCatalog.Infrastructure.Persistence.Configurations;

public sealed class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("Movies");

        builder.HasKey(movie => movie.Id);
        builder.Ignore(movie => movie.DomainEvents);

        builder.Property(movie => movie.Id)
            .ValueGeneratedNever();

        builder.Property(movie => movie.Title)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(movie => movie.OriginalTitle)
            .HasMaxLength(300);

        builder.Property(movie => movie.ReleaseYear)
            .IsRequired();

        builder.Property(movie => movie.CountryCode)
            .HasMaxLength(2)
            .IsRequired();

        builder.Property(movie => movie.OriginalLanguage)
            .HasMaxLength(35)
            .IsRequired();

        builder.Property(movie => movie.Director)
            .HasMaxLength(200);

        builder.Property(movie => movie.Synopsis)
            .HasMaxLength(4000);

        builder.Property(movie => movie.DurationMinutes);

        builder.Property(movie => movie.AgeRating)
            .HasMaxLength(32);

        builder.Property(movie => movie.CreatedAtUtc)
            .IsRequired();

        builder.OwnsMany(
            movie => movie.Genres,
            genres =>
            {
                genres.ToTable("MovieGenres");
                genres.WithOwner().HasForeignKey("MovieId");
                genres.Property<Guid>("MovieId");
                genres.Property(genre => genre.Name)
                    .HasMaxLength(100)
                    .IsRequired();
                genres.HasKey("MovieId", nameof(MovieGenre.Name));
            });

        builder.Navigation(movie => movie.Genres)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
