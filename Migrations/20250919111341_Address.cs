using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommercePlatform.Migrations
{
    /// <inheritdoc />
    public partial class Address : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DeliveryAddressId",
                table: "RecentCarts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecentCarts_DeliveryAddressId",
                table: "RecentCarts",
                column: "DeliveryAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecentCarts_Addresses_DeliveryAddressId",
                table: "RecentCarts",
                column: "DeliveryAddressId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecentCarts_Addresses_DeliveryAddressId",
                table: "RecentCarts");

            migrationBuilder.DropIndex(
                name: "IX_RecentCarts_DeliveryAddressId",
                table: "RecentCarts");

            migrationBuilder.DropColumn(
                name: "DeliveryAddressId",
                table: "RecentCarts");
        }
    }
}
