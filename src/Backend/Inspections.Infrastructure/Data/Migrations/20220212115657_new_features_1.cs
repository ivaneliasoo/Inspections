using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class new_features_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OperationalReadings",
                schema: "Inspections",
                table: "ReportsConfiguration",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TemplateName",
                schema: "Inspections",
                table: "ReportsConfiguration",
                type: "text",
                nullable: false,
                defaultValue: "print");

            migrationBuilder.AddColumn<int>(
                name: "ReportConfigurationId",
                schema: "Inspections",
                table: "Reports",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OperationalReadings",
                schema: "Inspections",
                table: "ReportsConfiguration");

            migrationBuilder.DropColumn(
                name: "TemplateName",
                schema: "Inspections",
                table: "ReportsConfiguration");

            migrationBuilder.DropColumn(
                name: "ReportConfigurationId",
                schema: "Inspections",
                table: "Reports");
        }
    }
}
