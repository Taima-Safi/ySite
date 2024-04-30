using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ySite.EF.Migrations
{
    /// <inheritdoc />
    public partial class Renamefilename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Posts",
                newName: "File");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Comments",
                newName: "File");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "File",
                table: "Posts",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "File",
                table: "Comments",
                newName: "Image");
        }
    }
}
