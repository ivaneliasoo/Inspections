using Inspections.Core.Domain;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class add_costsheet_options_field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<CSOptions>(
                name: "options",
                table: "CSTemplate",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<CSOptions>(
                name: "options",
                table: "CostSheet",
                type: "jsonb",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "options",
                table: "CSTemplate");

            migrationBuilder.DropColumn(
                name: "options",
                table: "CostSheet");
        }
    }
}
