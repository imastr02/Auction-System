using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auction_System.Migrations
{
    /// <inheritdoc />
    public partial class FixDeleteUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionEvents_AspNetUsers_SellerId",
                table: "AuctionEvents");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionEvents_AspNetUsers_SellerId",
                table: "AuctionEvents",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionEvents_AspNetUsers_SellerId",
                table: "AuctionEvents");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionEvents_AspNetUsers_SellerId",
                table: "AuctionEvents",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
