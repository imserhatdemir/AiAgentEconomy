using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AiAgentEconomy.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MarketplaceServiceRegistry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "service_vendors");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "service_vendors");

            migrationBuilder.CreateTable(
                name: "marketplace_services",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VendorId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServiceCode = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marketplace_services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_marketplace_services_service_vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "service_vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_marketplace_services_ServiceCode",
                table: "marketplace_services",
                column: "ServiceCode");

            migrationBuilder.CreateIndex(
                name: "IX_marketplace_services_VendorId_ServiceCode",
                table: "marketplace_services",
                columns: new[] { "VendorId", "ServiceCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "marketplace_services");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "service_vendors",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "service_vendors",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
