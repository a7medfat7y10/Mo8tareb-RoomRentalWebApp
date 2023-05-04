using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mo8tareb_RoomRentalWebApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserPaymentRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_AspNetUsers_AppUserId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Reservations_ReservationId",
                table: "Payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_AppUserId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Payment");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "payments");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_ReservationId",
                table: "payments",
                newName: "IX_payments_ReservationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_payments",
                table: "payments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_payments_Reservations_ReservationId",
                table: "payments",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payments_Reservations_ReservationId",
                table: "payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_payments",
                table: "payments");

            migrationBuilder.RenameTable(
                name: "payments",
                newName: "Payment");

            migrationBuilder.RenameIndex(
                name: "IX_payments_ReservationId",
                table: "Payment",
                newName: "IX_Payment_ReservationId");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Payment",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                table: "Payment",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_AppUserId",
                table: "Payment",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_AspNetUsers_AppUserId",
                table: "Payment",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Reservations_ReservationId",
                table: "Payment",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id");
        }
    }
}
