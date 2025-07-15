using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieApi.Migrations
{
    /// <inheritdoc />
    public partial class RenameMovieToVideoMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rename Movie table to VideoMovies
            migrationBuilder.RenameTable(
                name: "Movie",
                newName: "VideoMovies");

            // Rename MovieId columns to VideoMovieId in related tables
            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "MovieDetails",
                newName: "VideoMovieId");
            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "MovieReview",
                newName: "VideoMovieId");
            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "MovieActor",
                newName: "VideoMovieId");

            // Update foreign keys
            migrationBuilder.DropForeignKey(
                name: "FK_MovieDetails_Movie_MovieId",
                table: "MovieDetails");
            migrationBuilder.DropForeignKey(
                name: "FK_MovieReview_Movie_MovieId",
                table: "MovieReview");
            migrationBuilder.DropForeignKey(
                name: "FK_MovieActor_Movie_MovieId",
                table: "MovieActor");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieDetails_VideoMovies_VideoMovieId",
                table: "MovieDetails",
                column: "VideoMovieId",
                principalTable: "VideoMovies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_MovieReview_VideoMovies_VideoMovieId",
                table: "MovieReview",
                column: "VideoMovieId",
                principalTable: "VideoMovies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_MovieActor_VideoMovies_VideoMovieId",
                table: "MovieActor",
                column: "VideoMovieId",
                principalTable: "VideoMovies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // Update indexes
            migrationBuilder.RenameIndex(
                name: "IX_MovieReview_MovieId",
                table: "MovieReview",
                newName: "IX_MovieReview_VideoMovieId");
            migrationBuilder.RenameIndex(
                name: "IX_MovieDetails_MovieId",
                table: "MovieDetails",
                newName: "IX_MovieDetails_VideoMovieId");
            migrationBuilder.RenameIndex(
                name: "IX_MovieActor_MovieId",
                table: "MovieActor",
                newName: "IX_MovieActor_VideoMovieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert table name
            migrationBuilder.RenameTable(
                name: "VideoMovies",
                newName: "Movie");

            // Revert column names
            migrationBuilder.RenameColumn(
                name: "VideoMovieId",
                table: "MovieDetails",
                newName: "MovieId");
            migrationBuilder.RenameColumn(
                name: "VideoMovieId",
                table: "MovieReview",
                newName: "MovieId");
            migrationBuilder.RenameColumn(
                name: "VideoMovieId",
                table: "MovieActor",
                newName: "MovieId");

            // Revert foreign keys
            migrationBuilder.DropForeignKey(
                name: "FK_MovieDetails_VideoMovies_VideoMovieId",
                table: "MovieDetails");
            migrationBuilder.DropForeignKey(
                name: "FK_MovieReview_VideoMovies_VideoMovieId",
                table: "MovieReview");
            migrationBuilder.DropForeignKey(
                name: "FK_MovieActor_VideoMovies_VideoMovieId",
                table: "MovieActor");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieDetails_Movie_MovieId",
                table: "MovieDetails",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_MovieReview_Movie_MovieId",
                table: "MovieReview",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_MovieActor_Movie_MovieId",
                table: "MovieActor",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // Revert indexes
            migrationBuilder.RenameIndex(
                name: "IX_MovieReview_VideoMovieId",
                table: "MovieReview",
                newName: "IX_MovieReview_MovieId");
            migrationBuilder.RenameIndex(
                name: "IX_MovieDetails_VideoMovieId",
                table: "MovieDetails",
                newName: "IX_MovieDetails_MovieId");
            migrationBuilder.RenameIndex(
                name: "IX_MovieActor_VideoMovieId",
                table: "MovieActor",
                newName: "IX_MovieActor_MovieId");
        }
    }
}
