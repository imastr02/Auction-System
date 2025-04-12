using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auction_System.Migrations
{
    /// <inheritdoc />
    public partial class fixingAuctionEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AuctionEvents",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndingTime", "StartingTime" },
                values: new object[] { new TimeSpan(0, 10, 0, 0, 0), new TimeSpan(0, 9, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "AuctionEvents",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndingTime", "StartingTime" },
                values: new object[] { new TimeSpan(0, 13, 0, 0, 0), new TimeSpan(0, 12, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "AuctionEvents",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndingTime", "StartingTime" },
                values: new object[] { new TimeSpan(0, 16, 0, 0, 0), new TimeSpan(0, 15, 0, 0, 0) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AuctionEvents",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndingTime", "StartingTime" },
                values: new object[] { new TimeSpan(0, 0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "AuctionEvents",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndingTime", "StartingTime" },
                values: new object[] { new TimeSpan(0, 0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "AuctionEvents",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndingTime", "StartingTime" },
                values: new object[] { new TimeSpan(0, 0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 0) });
        }
    }
}
