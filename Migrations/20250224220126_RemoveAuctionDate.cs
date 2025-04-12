using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auction_System.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAuctionDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuctionDate",
                table: "AuctionEvents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AuctionDate",
                table: "AuctionEvents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AuctionEvents",
                keyColumn: "Id",
                keyValue: 1,
                column: "AuctionDate",
                value: null);

            migrationBuilder.UpdateData(
                table: "AuctionEvents",
                keyColumn: "Id",
                keyValue: 2,
                column: "AuctionDate",
                value: null);

            migrationBuilder.UpdateData(
                table: "AuctionEvents",
                keyColumn: "Id",
                keyValue: 3,
                column: "AuctionDate",
                value: null);
        }
    }
}
