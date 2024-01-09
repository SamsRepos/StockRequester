using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockRequester.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddEditedByUsersToTrTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EditedByUsersBlob",
                table: "TransferRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "TransferRequests",
                keyColumn: "Id",
                keyValue: 1,
                column: "EditedByUsersBlob",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditedByUsersBlob",
                table: "TransferRequests");
        }
    }
}
