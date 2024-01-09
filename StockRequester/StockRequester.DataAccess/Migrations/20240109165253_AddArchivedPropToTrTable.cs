using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockRequester.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddArchivedPropToTrTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "TransferRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "TransferRequests",
                keyColumn: "Id",
                keyValue: 1,
                column: "Archived",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Archived",
                table: "TransferRequests");
        }
    }
}
