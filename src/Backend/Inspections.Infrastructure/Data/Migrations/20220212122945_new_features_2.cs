using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class new_features_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DynamicFields",
                schema: "Inspections",
                table: "Reports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DynamicOperationalReadings",
                schema: "Inspections",
                table: "Reports",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DynamicFields",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "DynamicOperationalReadings",
                schema: "Inspections",
                table: "Reports");
        }
    }
}
