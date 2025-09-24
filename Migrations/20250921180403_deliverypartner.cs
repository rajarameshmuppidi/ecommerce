using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommercePlatform.Migrations
{
    /// <inheritdoc />
    public partial class deliverypartner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryPartners",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VehicleNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryPartners", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_DeliveryPartners_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "289ed434-6436-420b-8753-84addf50bc9a",
                column: "NormalizedName",
                value: "MODERATOR");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5a8e9d1b-3c7f-4a2d-b8e1-9f3c2a1b4d6e", "5a8e9d1b-3c7f-4a2d-b8e1-9f3c2a1b4d6e", "DeliveryPartner", "DELIVERYPARTNER" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryPartners");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5a8e9d1b-3c7f-4a2d-b8e1-9f3c2a1b4d6e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "289ed434-6436-420b-8753-84addf50bc9a",
                column: "NormalizedName",
                value: "Moderator");
        }
    }
}
