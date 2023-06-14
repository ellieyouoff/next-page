using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NP.Migrations
{
    /// <inheritdoc />
    public partial class mig5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "2",
                table: "Ratings",
                newName: "RatingValue");

            migrationBuilder.RenameColumn(
                name: "1",
                table: "Ratings",
                newName: "BookId");

            migrationBuilder.RenameColumn(
                name: "0",
                table: "Ratings",
                newName: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Ratings",
                newName: "0");

            migrationBuilder.RenameColumn(
                name: "RatingValue",
                table: "Ratings",
                newName: "2");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "Ratings",
                newName: "1");
        }
    }
}
