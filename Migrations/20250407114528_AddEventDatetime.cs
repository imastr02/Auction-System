using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auction_System.Migrations
{
    /// <inheritdoc />
    public partial class AddEventDatetime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AuctionDate",
                table: "AuctionEvents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuctionDate",
                table: "AuctionEvents");
        }
    }
}
