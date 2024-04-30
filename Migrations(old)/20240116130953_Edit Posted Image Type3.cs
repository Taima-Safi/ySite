using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ySite.EF.Migrations
{
    /// <inheritdoc />
    public partial class EditPostedImageType3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
               name: "Image",
               table: "Posts",
               type: "varbinary(max)",
               nullable: true);
        }
    }
}
