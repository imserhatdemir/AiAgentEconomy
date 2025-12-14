using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AiAgentEconomy.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MarketplaceTransactionValidation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MarketplaceServiceId",
                table: "transactions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "transactions",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitPriceCurrency",
                table: "transactions",
                type: "character varying(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VendorId",
                table: "transactions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_transactions_MarketplaceServiceId",
                table: "transactions",
                column: "MarketplaceServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_VendorId",
                table: "transactions",
                column: "VendorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_transactions_MarketplaceServiceId",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "IX_transactions_VendorId",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "MarketplaceServiceId",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "UnitPriceCurrency",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "VendorId",
                table: "transactions");
        }
    }
}
