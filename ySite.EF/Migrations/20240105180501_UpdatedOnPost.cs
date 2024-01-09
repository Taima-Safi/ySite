using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ySite.EF.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedOnPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Posts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FriendShips",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FriendId",
                table: "FriendShips",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_FriendShips_FriendId",
                table: "FriendShips",
                column: "FriendId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendShips_UserId",
                table: "FriendShips",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendShips_AspNetUsers_FriendId",
                table: "FriendShips",
                column: "FriendId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendShips_AspNetUsers_UserId",
                table: "FriendShips",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendShips_AspNetUsers_FriendId",
                table: "FriendShips");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendShips_AspNetUsers_UserId",
                table: "FriendShips");

            migrationBuilder.DropIndex(
                name: "IX_FriendShips_FriendId",
                table: "FriendShips");

            migrationBuilder.DropIndex(
                name: "IX_FriendShips_UserId",
                table: "FriendShips");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Posts");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FriendShips",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "FriendId",
                table: "FriendShips",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
