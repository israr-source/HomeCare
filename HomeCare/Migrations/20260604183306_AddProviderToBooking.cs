using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeCare.Migrations
{
    /// <inheritdoc />
    public partial class AddProviderToBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProviderId",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ProviderId",
                table: "Bookings",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_ProviderId",
                table: "Bookings",
                column: "ProviderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_ProviderId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_ProviderId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "Bookings");
        }
    }
}
