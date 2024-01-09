using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockRequester.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusChangedByUsersToTrTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EditedByUsersBlob",
                table: "TransferRequests",
                newName: "StatusChangedByUsersIdsBlob");

            migrationBuilder.AddColumn<string>(
                name: "EditedByUsersIdsBlob",
                table: "TransferRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "TransferRequests",
                keyColumn: "Id",
                keyValue: 1,
                column: "EditedByUsersIdsBlob",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditedByUsersIdsBlob",
                table: "TransferRequests");

            migrationBuilder.RenameColumn(
                name: "StatusChangedByUsersIdsBlob",
                table: "TransferRequests",
                newName: "EditedByUsersBlob");
        }
    }
}
