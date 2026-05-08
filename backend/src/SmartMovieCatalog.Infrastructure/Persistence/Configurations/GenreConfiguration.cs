using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartMovieCatalog.Domain.Movies;

namespace SmartMovieCatalog.Infrastructure.Persistence.Configurations;

public sealed class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("Genres");

        builder.HasKey(genre => genre.Id);
        builder.Ignore(genre => genre.DomainEvents);

        builder.Property(genre => genre.Id)
            .ValueGeneratedNever();

        builder.Property(genre => genre.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(genre => genre.NormalizedName)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(genre => genre.NormalizedName)
            .IsUnique();

        builder.Property(genre => genre.ExternalId);

        builder.HasIndex(genre => genre.ExternalId)
            .IsUnique();

        builder.HasData(
            new { Id = new Guid("8130f6e7-0b55-4383-ab8a-2419a4a0bf84"), Name = "Ação", NormalizedName = "AÇÃO", ExternalId = (int?)null },
            new { Id = new Guid("2ee73bcc-9d10-4881-8046-363802db5b61"), Name = "Aventura", NormalizedName = "AVENTURA", ExternalId = (int?)null },
            new { Id = new Guid("9f2d4781-975e-4b86-887a-eae96a06e93e"), Name = "Animação", NormalizedName = "ANIMAÇÃO", ExternalId = (int?)null },
            new { Id = new Guid("d0d2a285-d64c-4926-bb6e-bee246cb2066"), Name = "Comédia", NormalizedName = "COMÉDIA", ExternalId = (int?)null },
            new { Id = new Guid("91ebc5ab-6a87-45fe-bcf4-e98d1f1c1c47"), Name = "Crime", NormalizedName = "CRIME", ExternalId = (int?)null },
            new { Id = new Guid("cc60b818-3fe0-4a4c-904c-e6ff7c6bb14e"), Name = "Documentário", NormalizedName = "DOCUMENTÁRIO", ExternalId = (int?)null },
            new { Id = new Guid("671a6527-8d6a-4773-8755-0702cce3859b"), Name = "Drama", NormalizedName = "DRAMA", ExternalId = (int?)null },
            new { Id = new Guid("3dbe3e7c-b3a5-48d9-8539-c63f3263ca62"), Name = "Família", NormalizedName = "FAMÍLIA", ExternalId = (int?)null },
            new { Id = new Guid("c5016fec-223b-4245-9619-4ebd9ed61616"), Name = "Fantasia", NormalizedName = "FANTASIA", ExternalId = (int?)null },
            new { Id = new Guid("0bdb1338-15b8-4388-a600-4d6c1da3c9ac"), Name = "História", NormalizedName = "HISTÓRIA", ExternalId = (int?)null },
            new { Id = new Guid("63186b3c-7ee8-4e27-b04e-93d38213b18f"), Name = "Terror", NormalizedName = "TERROR", ExternalId = (int?)null },
            new { Id = new Guid("40d64d12-8dad-495d-9c8c-be79913a64a1"), Name = "Música", NormalizedName = "MÚSICA", ExternalId = (int?)null },
            new { Id = new Guid("93685229-5cbb-4d6b-8583-320faff48c3a"), Name = "Mistério", NormalizedName = "MISTÉRIO", ExternalId = (int?)null },
            new { Id = new Guid("b202ae42-19a0-4919-8531-4bd16535ba07"), Name = "Romance", NormalizedName = "ROMANCE", ExternalId = (int?)null },
            new { Id = new Guid("65836d3b-9de2-4652-80d8-9f0a17da74e4"), Name = "Ficção científica", NormalizedName = "FICÇÃO CIENTÍFICA", ExternalId = (int?)null },
            new { Id = new Guid("1b3b0c5c-b8a4-4a99-8ed3-c7829b1e04ba"), Name = "Cinema TV", NormalizedName = "CINEMA TV", ExternalId = (int?)null },
            new { Id = new Guid("4a46f05d-370f-4a50-82e2-3d2160597c13"), Name = "Thriller", NormalizedName = "THRILLER", ExternalId = (int?)null },
            new { Id = new Guid("ccfbe9c3-be3b-4a83-8d25-0788a19490e6"), Name = "Guerra", NormalizedName = "GUERRA", ExternalId = (int?)null },
            new { Id = new Guid("ea89b466-98bd-4761-b883-a33f0effac99"), Name = "Faroeste", NormalizedName = "FAROESTE", ExternalId = (int?)null });
    }
}
