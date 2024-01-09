using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockRequester.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByUserToTrTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "TransferRequests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "TransferRequests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedByUserId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_TransferRequests_CreatedByUserId",
                table: "TransferRequests",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransferRequests_AspNetUsers_CreatedByUserId",
                table: "TransferRequests",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransferRequests_AspNetUsers_CreatedByUserId",
                table: "TransferRequests");

            migrationBuilder.DropIndex(
                name: "IX_TransferRequests_CreatedByUserId",
                table: "TransferRequests");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "TransferRequests");
        }
    }
}
