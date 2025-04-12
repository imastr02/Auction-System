using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auction_System.Migrations
{
    /// <inheritdoc />
    public partial class AddIsEndedEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnded",
                table: "AuctionEvents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AuctionEvents",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsEnded",
                value: false);

            migrationBuilder.UpdateData(
                table: "AuctionEvents",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsEnded",
                value: false);

            migrationBuilder.UpdateData(
                table: "AuctionEvents",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsEnded",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnded",
                table: "AuctionEvents");
        }
    }
}
