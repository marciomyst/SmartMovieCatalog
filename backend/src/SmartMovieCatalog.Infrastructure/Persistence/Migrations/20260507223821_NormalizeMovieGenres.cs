using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartMovieCatalog.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeMovieGenres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NormalizedName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ExternalId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Genres_ExternalId",
                table: "Genres",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genres_NormalizedName",
                table: "Genres",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.Sql(
                """
                INSERT INTO "Genres" ("Id", "Name", "NormalizedName", "ExternalId")
                SELECT
                    CAST(CONCAT(
                        SUBSTRING(MD5("NormalizedName"), 1, 8), '-',
                        SUBSTRING(MD5("NormalizedName"), 9, 4), '-',
                        SUBSTRING(MD5("NormalizedName"), 13, 4), '-',
                        SUBSTRING(MD5("NormalizedName"), 17, 4), '-',
                        SUBSTRING(MD5("NormalizedName"), 21, 12)
                    ) AS uuid) AS "Id",
                    MIN("Name") AS "Name",
                    "NormalizedName",
                    NULL AS "ExternalId"
                FROM (
                    SELECT
                        TRIM("Name") AS "Name",
                        UPPER(TRIM("Name")) AS "NormalizedName"
                    FROM "MovieGenres"
                    WHERE TRIM("Name") <> ''
                ) AS "SourceGenres"
                GROUP BY "NormalizedName";
                """);

            migrationBuilder.AddColumn<Guid>(
                name: "GenreId",
                table: "MovieGenres",
                type: "uuid",
                nullable: true);

            migrationBuilder.Sql(
                """
                UPDATE "MovieGenres" AS "MovieGenre"
                SET "GenreId" = "Genre"."Id"
                FROM "Genres" AS "Genre"
                WHERE UPPER(TRIM("MovieGenre"."Name")) = "Genre"."NormalizedName";
                """);

            migrationBuilder.Sql(
                """
                DELETE FROM "MovieGenres"
                WHERE "GenreId" IS NULL;
                """);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Genres_GenreId_Backfill",
                table: "MovieGenres",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id");

            migrationBuilder.Sql(
                """
                DELETE FROM "MovieGenres" AS "DuplicateMovieGenre"
                USING "MovieGenres" AS "KeptMovieGenre"
                WHERE
                    "DuplicateMovieGenre"."ctid" < "KeptMovieGenre"."ctid" AND
                    "DuplicateMovieGenre"."MovieId" = "KeptMovieGenre"."MovieId" AND
                    "DuplicateMovieGenre"."GenreId" = "KeptMovieGenre"."GenreId";
                """);

            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Genres_GenreId_Backfill",
                table: "MovieGenres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieGenres",
                table: "MovieGenres");

            migrationBuilder.AlterColumn<Guid>(
                name: "GenreId",
                table: "MovieGenres",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.DropColumn(
                name: "Name",
                table: "MovieGenres");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieGenres",
                table: "MovieGenres",
                columns: new[] { "MovieId", "GenreId" });

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenres_GenreId",
                table: "MovieGenres",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Genres_GenreId",
                table: "MovieGenres",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Genres_GenreId",
                table: "MovieGenres");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MovieGenres",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.Sql(
                """
                UPDATE "MovieGenres" AS "MovieGenre"
                SET "Name" = "Genre"."Name"
                FROM "Genres" AS "Genre"
                WHERE "MovieGenre"."GenreId" = "Genre"."Id";
                """);

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieGenres",
                table: "MovieGenres");

            migrationBuilder.DropIndex(
                name: "IX_MovieGenres_GenreId",
                table: "MovieGenres");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "MovieGenres");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MovieGenres",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieGenres",
                table: "MovieGenres",
                columns: new[] { "MovieId", "Name" });

            migrationBuilder.DropTable(
                name: "Genres");
        }
    }
}
