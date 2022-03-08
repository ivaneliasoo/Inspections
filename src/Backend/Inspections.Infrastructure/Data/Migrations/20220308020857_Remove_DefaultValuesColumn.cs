using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class Remove_DefaultValuesColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultValues",
                schema: "Inspections",
                table: "FormDefinition");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<JsonDocument>(
                name: "DefaultValues",
                schema: "Inspections",
                table: "FormDefinition",
                type: "jsonb",
                nullable: true);
        }
    }
}
