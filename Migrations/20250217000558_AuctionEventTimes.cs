using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auction_System.Migrations
{
    /// <inheritdoc />
    public partial class AuctionEventTimes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AuctionEvents",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndingTime", "StartingTime" },
                values: new object[] { new DateTime(2025, 2, 17, 10, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 2, 17, 9, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "AuctionEvents",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndingTime", "StartingTime" },
                values: new object[] { new DateTime(2025, 2, 17, 13, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 2, 17, 12, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "AuctionEvents",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndingTime", "StartingTime" },
                values: new object[] { new DateTime(2025, 2, 17, 16, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 2, 17, 15, 0, 0, 0, DateTimeKind.Local) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AuctionEvents",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndingTime", "StartingTime" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "AuctionEvents",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndingTime", "StartingTime" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "AuctionEvents",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "EndingTime", "StartingTime" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
