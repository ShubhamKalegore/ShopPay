using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopPay.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStripeCustomerEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "stripe_customers",
                columns: table => new
                {
                    stripe_customer_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stripe_customers", x => x.stripe_customer_id);
                    table.ForeignKey(
                        name: "FK_stripe_customers_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_stripe_customers_user_id",
                table: "stripe_customers",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "stripe_customers");
        }
    }
}
