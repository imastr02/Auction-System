using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Auction_System.Migrations
{
    /// <inheritdoc />
    public partial class FixAuctionEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionEvents_Items_ItemId1",
                table: "AuctionEvents");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_AuctionEvents_ItemId1",
                table: "AuctionEvents");

            migrationBuilder.DeleteData(
                table: "AuctionEvents",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AuctionEvents",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AuctionEvents",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "EndingTime",
                table: "AuctionEvents");

            migrationBuilder.DropColumn(
                name: "IsEnded",
                table: "AuctionEvents");

            migrationBuilder.DropColumn(
                name: "ItemId1",
                table: "AuctionEvents");

            migrationBuilder.DropColumn(
                name: "StartingTime",
                table: "AuctionEvents");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "AuctionEvents",
                newName: "Status");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AuctionEvents",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AuctionEvents",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "AuctionEvents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SellerId",
                table: "AuctionEvents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "AuctionEvents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_AuctionEvents_SellerId",
                table: "AuctionEvents",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionEvents_AspNetUsers_SellerId",
                table: "AuctionEvents",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionEvents_AspNetUsers_SellerId",
                table: "AuctionEvents");

            migrationBuilder.DropIndex(
                name: "IX_AuctionEvents_SellerId",
                table: "AuctionEvents");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AuctionEvents");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "AuctionEvents");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "AuctionEvents");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "AuctionEvents");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "AuctionEvents",
                newName: "ItemId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AuctionEvents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndingTime",
                table: "AuctionEvents",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<bool>(
                name: "IsEnded",
                table: "AuctionEvents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ItemId1",
                table: "AuctionEvents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartingTime",
                table: "AuctionEvents",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AuctionEvents",
                columns: new[] { "Id", "EndingTime", "IsEnded", "ItemId", "ItemId1", "Name", "StartingTime" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 10, 0, 0, 0), false, 0, null, "9 AM - 10 AM", new TimeSpan(0, 9, 0, 0, 0) },
                    { 2, new TimeSpan(0, 13, 0, 0, 0), false, 0, null, "12 PM - 1 PM", new TimeSpan(0, 12, 0, 0, 0) },
                    { 3, new TimeSpan(0, 16, 0, 0, 0), false, 0, null, "3 PM - 4 PM", new TimeSpan(0, 15, 0, 0, 0) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuctionEvents_ItemId1",
                table: "AuctionEvents",
                column: "ItemId1");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RecipientId",
                table: "Messages",
                column: "RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionEvents_Items_ItemId1",
                table: "AuctionEvents",
                column: "ItemId1",
                principalTable: "Items",
                principalColumn: "Id");
        }
    }
}
