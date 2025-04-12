using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auction_System.Migrations
{
    /// <inheritdoc />
    public partial class ExchangedUserIdtouyerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WatchLists_AspNetUsers_UserId",
                table: "WatchLists");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "WatchLists",
                newName: "BuyerId");

            migrationBuilder.RenameIndex(
                name: "IX_WatchLists_UserId",
                table: "WatchLists",
                newName: "IX_WatchLists_BuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_WatchLists_AspNetUsers_BuyerId",
                table: "WatchLists",
                column: "BuyerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WatchLists_AspNetUsers_BuyerId",
                table: "WatchLists");

            migrationBuilder.RenameColumn(
                name: "BuyerId",
                table: "WatchLists",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_WatchLists_BuyerId",
                table: "WatchLists",
                newName: "IX_WatchLists_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WatchLists_AspNetUsers_UserId",
                table: "WatchLists",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
