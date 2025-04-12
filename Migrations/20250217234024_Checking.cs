using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auction_System.Migrations
{
    /// <inheritdoc />
    public partial class Checking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartingTime",
                table: "AuctionEvents",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndingTime",
                table: "AuctionEvents",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "AuctionDate",
                table: "AuctionEvents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AuctionEvents",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AuctionDate", "EndingTime", "StartingTime" },
                values: new object[] { null, new TimeSpan(0, 0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "AuctionEvents",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AuctionDate", "EndingTime", "StartingTime" },
                values: new object[] { null, new TimeSpan(0, 0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 0) });

            migrationBuilder.UpdateData(
                table: "AuctionEvents",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AuctionDate", "EndingTime", "StartingTime" },
                values: new object[] { null, new TimeSpan(0, 0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 0) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuctionDate",
                table: "AuctionEvents");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartingTime",
                table: "AuctionEvents",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndingTime",
                table: "AuctionEvents",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

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
    }
}
