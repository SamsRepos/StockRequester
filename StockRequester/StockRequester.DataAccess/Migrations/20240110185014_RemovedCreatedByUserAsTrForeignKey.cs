using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockRequester.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemovedCreatedByUserAsTrForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransferRequests_AspNetUsers_CreatedByUserId",
                table: "TransferRequests");

            migrationBuilder.DropIndex(
                name: "IX_TransferRequests_CreatedByUserId",
                table: "TransferRequests");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "TransferRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "TransferRequests",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransferRequests_CreatedByUserId",
                table: "TransferRequests",
                column: "CreatedByUserId",
                unique: true,
                filter: "[CreatedByUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_TransferRequests_AspNetUsers_CreatedByUserId",
                table: "TransferRequests",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
