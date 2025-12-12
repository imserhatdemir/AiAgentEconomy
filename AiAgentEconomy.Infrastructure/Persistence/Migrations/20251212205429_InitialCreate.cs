using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AiAgentEconomy.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "agent_policies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaxPerTransaction = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DailyLimit = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    AllowedVendorsCsv = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    AllowedServicesCsv = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    DailyWindowDate = table.Column<DateOnly>(type: "date", nullable: true),
                    SpentInDailyWindow = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agent_policies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "agent_transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentId = table.Column<Guid>(type: "uuid", nullable: false),
                    WalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServiceVendorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    BlockchainTxHash = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    MetadataJson = table.Column<string>(type: "text", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agent_transactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "service_vendors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    WalletAddress = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_vendors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "wallets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Chain = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wallets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "agents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Goal = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    RiskLevel = table.Column<int>(type: "integer", nullable: false),
                    MonthlyBudget = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    SpentThisMonth = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    WalletId = table.Column<Guid>(type: "uuid", nullable: true),
                    PolicyId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_agents_agent_policies_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "agent_policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_agents_wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_agent_policies_AgentId",
                table: "agent_policies",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_agent_transactions_AgentId",
                table: "agent_transactions",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_agent_transactions_Status",
                table: "agent_transactions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_agents_PolicyId",
                table: "agents",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_agents_Status",
                table: "agents",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_agents_UserId",
                table: "agents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_agents_WalletId",
                table: "agents",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_service_vendors_IsActive",
                table: "service_vendors",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_service_vendors_Name",
                table: "service_vendors",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_wallets_Address",
                table: "wallets",
                column: "Address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_wallets_UserId",
                table: "wallets",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "agent_transactions");

            migrationBuilder.DropTable(
                name: "agents");

            migrationBuilder.DropTable(
                name: "service_vendors");

            migrationBuilder.DropTable(
                name: "agent_policies");

            migrationBuilder.DropTable(
                name: "wallets");
        }
    }
}
