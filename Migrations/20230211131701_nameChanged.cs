using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextPage.Migrations
{
    /// <inheritdoc />
    public partial class nameChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Posted_Date",
                table: "Books",
                newName: "PostedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostedDate",
                table: "Books",
                newName: "Posted_Date");
        }
    }
}
