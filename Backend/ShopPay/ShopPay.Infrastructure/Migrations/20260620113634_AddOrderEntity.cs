using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopPay.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_orders_billing_address_id",
                table: "orders",
                column: "billing_address_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_shipping_address_id",
                table: "orders",
                column: "shipping_address_id");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_addresses_billing_address_id",
                table: "orders",
                column: "billing_address_id",
                principalTable: "addresses",
                principalColumn: "address_id");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_addresses_shipping_address_id",
                table: "orders",
                column: "shipping_address_id",
                principalTable: "addresses",
                principalColumn: "address_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_addresses_billing_address_id",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_addresses_shipping_address_id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_billing_address_id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_shipping_address_id",
                table: "orders");
        }
    }
}
