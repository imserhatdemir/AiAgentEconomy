using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AiAgentEconomy.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionVendorService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ServiceCode",
                table: "transactions",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vendor",
                table: "transactions",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_transactions_ServiceCode",
                table: "transactions",
                column: "ServiceCode");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_Vendor",
                table: "transactions",
                column: "Vendor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_transactions_ServiceCode",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "IX_transactions_Vendor",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "ServiceCode",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "Vendor",
                table: "transactions");
        }
    }
}
