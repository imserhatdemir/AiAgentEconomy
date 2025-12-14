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
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "marketplace_services",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "marketplace_services");
        }
    }
}
