using Microsoft.EntityFrameworkCore.Migrations;

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class add_new_opre_fields_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EarthFaultEF",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "EarthFaultEFAt",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "EarthFaultEIR",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "EarthFaultEIRAt",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "EarthFaultRoob",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "OverCurrentDTL",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "OverCurrentDTLAt",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "OverCurrentIDTML",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "OverCurrentIDTMLAt",
                table: "OperationalReadings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EarthFaultEF",
                table: "OperationalReadings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EarthFaultEFAt",
                table: "OperationalReadings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EarthFaultEIR",
                table: "OperationalReadings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EarthFaultEIRAt",
                table: "OperationalReadings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EarthFaultRoob",
                table: "OperationalReadings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OverCurrentDTL",
                table: "OperationalReadings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OverCurrentDTLAt",
                table: "OperationalReadings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OverCurrentIDTML",
                table: "OperationalReadings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OverCurrentIDTMLAt",
                table: "OperationalReadings",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
