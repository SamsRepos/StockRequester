using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockRequester.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DefaultItemDescriptionAddToCompanyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultItemDescription",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1,
                column: "DefaultItemDescription",
                value: null);

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 2,
                column: "DefaultItemDescription",
                value: null);

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 3,
                column: "DefaultItemDescription",
                value: null);

            migrationBuilder.UpdateData(
                table: "TransferRequests",
                keyColumn: "Id",
                keyValue: 1,
                column: "FirstCreatedDateTime",
                value: new DateTime(2024, 1, 11, 12, 19, 38, 996, DateTimeKind.Local).AddTicks(7451));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultItemDescription",
                table: "Companies");

            migrationBuilder.UpdateData(
                table: "TransferRequests",
                keyColumn: "Id",
                keyValue: 1,
                column: "FirstCreatedDateTime",
                value: null);
        }
    }
}
