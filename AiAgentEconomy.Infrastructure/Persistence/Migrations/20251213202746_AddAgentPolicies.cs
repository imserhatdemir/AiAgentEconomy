using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AiAgentEconomy.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAgentPolicies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_agent_policies_AgentId",
                table: "agent_policies");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "agent_policies",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "AllowedVendorsCsv",
                table: "agent_policies",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AllowedServicesCsv",
                table: "agent_policies",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "agent_policies",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "agent_policies",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_agent_policies_AgentId",
                table: "agent_policies",
                column: "AgentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_agent_policies_IsActive",
                table: "agent_policies",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_agent_policies_Name",
                table: "agent_policies",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_agent_policies_AgentId",
                table: "agent_policies");

            migrationBuilder.DropIndex(
                name: "IX_agent_policies_IsActive",
                table: "agent_policies");

            migrationBuilder.DropIndex(
                name: "IX_agent_policies_Name",
                table: "agent_policies");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "agent_policies");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "agent_policies");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "agent_policies",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(16)",
                oldMaxLength: 16);

            migrationBuilder.AlterColumn<string>(
                name: "AllowedVendorsCsv",
                table: "agent_policies",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AllowedServicesCsv",
                table: "agent_policies",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_agent_policies_AgentId",
                table: "agent_policies",
                column: "AgentId");
        }
    }
}
