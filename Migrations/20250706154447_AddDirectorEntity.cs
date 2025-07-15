using Microsoft.EntityFrameworkCore.Migrations;
using Movie.Data;

#nullable disable

namespace MovieApi.Migrations
{
    /// <inheritdoc />
    public partial class AddDirectorEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DirectorId",
                table: "Movie",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Director",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Director", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movie_DirectorId",
                table: "Movie",
                column: "DirectorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Director_DirectorId",
                table: "Movie",
                column: "DirectorId",
                principalTable: "Director",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Director_DirectorId",
                table: "Movie");

            migrationBuilder.DropTable(
                name: "Director");

            migrationBuilder.DropIndex(
                name: "IX_Movie_DirectorId",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "DirectorId",
                table: "Movie");
        }
    }
}
