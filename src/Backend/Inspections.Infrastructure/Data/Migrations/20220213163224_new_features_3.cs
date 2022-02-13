using Inspections.Core.Domain.ReportConfigurationAggregate;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class new_features_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalFileds",
                schema: "Inspections",
                table: "ReportsConfiguration");

            migrationBuilder.DropColumn(
                name: "OperationalReadings",
                schema: "Inspections",
                table: "ReportsConfiguration");

            migrationBuilder.AddColumn<DynamicFields>(
                name: "AdditionalFields2",
                schema: "Inspections",
                table: "ReportsConfiguration",
                type: "jsonb",
                nullable: true,
                defaultValueSql: "'{ \"FieldsDefinitions\": null }'::jsonb");

            migrationBuilder.AddColumn<DynamicFields>(
                name: "OperationalReadings2",
                schema: "Inspections",
                table: "ReportsConfiguration",
                type: "jsonb",
                nullable: true,
                defaultValueSql: "'{ \"FieldsDefinitions\": null }'::jsonb");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalFields2",
                schema: "Inspections",
                table: "ReportsConfiguration");

            migrationBuilder.DropColumn(
                name: "OperationalReadings2",
                schema: "Inspections",
                table: "ReportsConfiguration");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalFileds",
                schema: "Inspections",
                table: "ReportsConfiguration",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OperationalReadings",
                schema: "Inspections",
                table: "ReportsConfiguration",
                type: "text",
                nullable: true);
        }
    }
}
