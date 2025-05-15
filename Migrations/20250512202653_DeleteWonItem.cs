using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auction_System.Migrations
{
    /// <inheritdoc />
    public partial class DeleteWonItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_AspNetUsers_WinnerId",
                table: "Items");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_AspNetUsers_WinnerId",
                table: "Items",
                column: "WinnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_AspNetUsers_WinnerId",
                table: "Items");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_AspNetUsers_WinnerId",
                table: "Items",
                column: "WinnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
