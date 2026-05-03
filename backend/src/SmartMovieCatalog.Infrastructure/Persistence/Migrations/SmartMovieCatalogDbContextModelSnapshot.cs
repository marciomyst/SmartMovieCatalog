using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartMovieCatalog.Infrastructure.Persistence;

#nullable disable

namespace SmartMovieCatalog.Infrastructure.Persistence.Migrations;

[DbContext(typeof(SmartMovieCatalogDbContext))]
public class SmartMovieCatalogDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "10.0.4")
            .HasAnnotation("Relational:MaxIdentifierLength", 63);

        NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

        modelBuilder.Entity("SmartMovieCatalog.Domain.Users.User", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedNever()
                .HasColumnType("uuid");

            b.Property<DateTimeOffset>("CreatedAtUtc")
                .HasColumnType("timestamp with time zone");

            b.Property<string>("Email")
                .IsRequired()
                .HasMaxLength(320)
                .HasColumnType("character varying(320)");

            b.Property<bool>("IsActive")
                .HasColumnType("boolean");

            b.Property<bool>("MustChangePasswordOnFirstLogin")
                .HasColumnType("boolean");

            b.Property<string>("Name")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("character varying(200)");

            b.Property<string>("NormalizedEmail")
                .IsRequired()
                .HasMaxLength(320)
                .HasColumnType("character varying(320)");

            b.Property<string>("PasswordHash")
                .IsRequired()
                .HasMaxLength(512)
                .HasColumnType("character varying(512)");

            b.Property<DateTimeOffset?>("RemovedAtUtc")
                .HasColumnType("timestamp with time zone");

            b.Property<DateTimeOffset?>("UpdatedAtUtc")
                .HasColumnType("timestamp with time zone");

            b.HasKey("Id");

            b.HasIndex("NormalizedEmail")
                .IsUnique();

            b.ToTable("Users", (string)null);
        });

        modelBuilder.Entity("SmartMovieCatalog.Domain.Users.User", b =>
        {
            b.OwnsMany("SmartMovieCatalog.Domain.Users.UserRole", "Roles", b1 =>
            {
                b1.Property<Guid>("UserId")
                    .HasColumnType("uuid");

                b1.Property<string>("Name")
                    .HasMaxLength(64)
                    .HasColumnType("character varying(64)")
                    .HasColumnName("Role");

                b1.HasKey("UserId", "Name");

                b1.ToTable("UserRoles", (string)null);

                b1.WithOwner()
                    .HasForeignKey("UserId");
            });

            b.Navigation("Roles");
        });
#pragma warning restore 612, 618
    }
}
