using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auction_System.Migrations
{
    /// <inheritdoc />
    public partial class AddWinner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSold",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "SoldPrice",
                table: "Items",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "WinnerId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_WinnerId",
                table: "Items",
                column: "WinnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_AspNetUsers_WinnerId",
                table: "Items",
                column: "WinnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_AspNetUsers_WinnerId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_WinnerId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "IsSold",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "SoldPrice",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "WinnerId",
                table: "Items");
        }
    }
}
