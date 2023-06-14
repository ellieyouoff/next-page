using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NP.Migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings");


            migrationBuilder.RenameColumn(
                name: "RatingValue",
                table: "Ratings",
                newName: "2");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "Ratings",
                newName: "1");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Ratings",
                newName: "0");

            migrationBuilder.AlterColumn<double>(
                name: "2",
                table: "Ratings",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "1",
                table: "Ratings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "0",
                table: "Ratings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Ratings",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "BookId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BookId",
                table: "AspNetUsers",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Books_BookId",
                table: "AspNetUsers",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Books_BookId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BookId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "AspNetUsers");

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

            migrationBuilder.AlterColumn<int>(
                name: "RatingValue",
                table: "Ratings",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "BookId",
                table: "Ratings",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Ratings",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");


            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings",
                columns: new[] { "UserId", "BookId" });


        }
    }
}
