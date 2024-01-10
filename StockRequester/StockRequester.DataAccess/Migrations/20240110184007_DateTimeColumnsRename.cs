using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockRequester.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DateTimeColumnsRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastModified",
                table: "TransferRequests",
                newName: "LastModifiedDateTime");

            migrationBuilder.RenameColumn(
                name: "FirstCreated",
                table: "TransferRequests",
                newName: "FirstCreatedDateTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastModifiedDateTime",
                table: "TransferRequests",
                newName: "LastModified");

            migrationBuilder.RenameColumn(
                name: "FirstCreatedDateTime",
                table: "TransferRequests",
                newName: "FirstCreated");
        }
    }
}
