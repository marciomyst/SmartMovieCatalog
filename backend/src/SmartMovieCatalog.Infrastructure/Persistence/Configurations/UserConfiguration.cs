using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartMovieCatalog.Domain.Users;

namespace SmartMovieCatalog.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(user => user.Id);
        builder.Ignore(user => user.DomainEvents);

        builder.Property(user => user.Id)
            .ValueGeneratedNever();

        builder.Property(user => user.Email)
            .HasConversion(
                email => email.Value,
                value => EmailAddress.Create(value))
            .HasMaxLength(320)
            .IsRequired();

        builder.Property(user => user.NormalizedEmail)
            .HasMaxLength(320)
            .IsRequired();

        builder.HasIndex(user => user.NormalizedEmail)
            .IsUnique();

        builder.Property(user => user.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(user => user.PasswordHash)
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(user => user.IsActive)
            .IsRequired();

        builder.Property(user => user.RemovedAtUtc);

        builder.Property(user => user.MustChangePasswordOnFirstLogin)
            .IsRequired();

        builder.Property(user => user.CreatedAtUtc)
            .IsRequired();

        builder.Property(user => user.UpdatedAtUtc);

        builder.OwnsMany(
            user => user.Roles,
            roles =>
            {
                roles.ToTable("UserRoles");
                roles.WithOwner().HasForeignKey("UserId");
                roles.Property<Guid>("UserId");
                roles.Property(role => role.Name)
                    .HasColumnName("Role")
                    .HasMaxLength(64)
                    .IsRequired();
                roles.HasKey("UserId", nameof(UserRole.Name));
            });

        builder.Navigation(user => user.Roles)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
