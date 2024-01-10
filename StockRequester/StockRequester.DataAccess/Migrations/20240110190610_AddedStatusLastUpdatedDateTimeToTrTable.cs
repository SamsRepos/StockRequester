using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockRequester.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedStatusLastUpdatedDateTimeToTrTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastModifiedDateTime",
                table: "TransferRequests",
                newName: "StatusLastUpdatedDateTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "DetailsLastEditedDateTime",
                table: "TransferRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "TransferRequests",
                keyColumn: "Id",
                keyValue: 1,
                column: "DetailsLastEditedDateTime",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DetailsLastEditedDateTime",
                table: "TransferRequests");

            migrationBuilder.RenameColumn(
                name: "StatusLastUpdatedDateTime",
                table: "TransferRequests",
                newName: "LastModifiedDateTime");
        }
    }
}
