using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopPay.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    invoice_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stripe_invoice_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    stripe_invoice_url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    billing_id = table.Column<int>(type: "int", nullable: true),
                    subscription_id = table.Column<int>(type: "int", nullable: true),
                    amount_due = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    amount_paid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    invoice_status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoices", x => x.invoice_id);
                    table.ForeignKey(
                        name: "FK_invoices_billing_billing_id",
                        column: x => x.billing_id,
                        principalTable: "billing",
                        principalColumn: "billing_id");
                    table.ForeignKey(
                        name: "FK_invoices_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_invoices_billing_id",
                table: "invoices",
                column: "billing_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_user_id",
                table: "invoices",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "invoices");
        }
    }
}
