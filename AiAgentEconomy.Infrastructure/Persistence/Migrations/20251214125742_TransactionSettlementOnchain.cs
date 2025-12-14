using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AiAgentEconomy.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TransactionSettlementOnchain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlockchainTxHash",
                table: "transactions",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Chain",
                table: "transactions",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExplorerUrl",
                table: "transactions",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FailedAtUtc",
                table: "transactions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FailureReason",
                table: "transactions",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Network",
                table: "transactions",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SettledAtUtc",
                table: "transactions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedAtUtc",
                table: "transactions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_transactions_BlockchainTxHash",
                table: "transactions",
                column: "BlockchainTxHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_transactions_BlockchainTxHash",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "BlockchainTxHash",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "Chain",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "ExplorerUrl",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "FailedAtUtc",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "FailureReason",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "Network",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "SettledAtUtc",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "SubmittedAtUtc",
                table: "transactions");
        }
    }
}
