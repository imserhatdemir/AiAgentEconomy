using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AiAgentEconomy.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AgentWalletLinking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "wallets",
                newName: "AgentId");

            migrationBuilder.RenameIndex(
                name: "IX_wallets_UserId",
                table: "wallets",
                newName: "IX_wallets_AgentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AgentId",
                table: "wallets",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_wallets_AgentId",
                table: "wallets",
                newName: "IX_wallets_UserId");
        }
    }
}
