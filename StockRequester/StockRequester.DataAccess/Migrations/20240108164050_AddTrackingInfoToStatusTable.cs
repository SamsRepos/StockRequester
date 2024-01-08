using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockRequester.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTrackingInfoToStatusTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrackingInfo",
                table: "RequestStatuses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "RequestStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "TrackingInfo",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackingInfo",
                table: "RequestStatuses");
        }
    }
}
