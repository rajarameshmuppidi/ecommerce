using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommercePlatform.Migrations
{
    /// <inheritdoc />
    public partial class deliverypartnerAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryPartnerAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeliveryPartnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PickedUpDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryPartnerAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryPartnerAssignments_DeliveryPartners_DeliveryPartnerId",
                        column: x => x.DeliveryPartnerId,
                        principalTable: "DeliveryPartners",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeliveryPartnerAssignments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryPartnerAssignments_DeliveryPartnerId",
                table: "DeliveryPartnerAssignments",
                column: "DeliveryPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryPartnerAssignments_OrderId",
                table: "DeliveryPartnerAssignments",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryPartnerAssignments");
        }
    }
}
