using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AiAgentEconomy.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixAgentWalletRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_agents_wallets_WalletId",
                table: "agents");

            migrationBuilder.DropIndex(
                name: "IX_wallets_AgentId",
                table: "wallets");

            migrationBuilder.DropIndex(
                name: "IX_agents_WalletId",
                table: "agents");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "agents");

            migrationBuilder.CreateIndex(
                name: "IX_wallets_AgentId",
                table: "wallets",
                column: "AgentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_wallets_agents_AgentId",
                table: "wallets",
                column: "AgentId",
                principalTable: "agents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_wallets_agents_AgentId",
                table: "wallets");

            migrationBuilder.DropIndex(
                name: "IX_wallets_AgentId",
                table: "wallets");

            migrationBuilder.AddColumn<Guid>(
                name: "WalletId",
                table: "agents",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_wallets_AgentId",
                table: "wallets",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_agents_WalletId",
                table: "agents",
                column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_agents_wallets_WalletId",
                table: "agents",
                column: "WalletId",
                principalTable: "wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
