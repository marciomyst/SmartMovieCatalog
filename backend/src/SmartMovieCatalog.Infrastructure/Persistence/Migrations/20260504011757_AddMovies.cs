using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartMovieCatalog.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMovies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    OriginalTitle = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    ReleaseYear = table.Column<int>(type: "integer", nullable: false),
                    CountryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    OriginalLanguage = table.Column<string>(type: "character varying(35)", maxLength: 35, nullable: false),
                    Director = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Synopsis = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: true),
                    AgeRating = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MovieGenres",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MovieId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieGenres", x => new { x.MovieId, x.Name });
                    table.ForeignKey(
                        name: "FK_MovieGenres_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieGenres");

            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
