using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommercePlatform.Migrations
{
    /// <inheritdoc />
    public partial class cartPaymentStatusMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "PaymentMethod",
                table: "RecentCarts",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "PaymentStatus",
                table: "RecentCarts",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "RecentCarts");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "RecentCarts");
        }
    }
}
