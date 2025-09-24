using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommercePlatform.Migrations
{
    /// <inheritdoc />
    public partial class paymentMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "PaymentMethod",
                table: "Orders",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Orders");
        }
    }
}
