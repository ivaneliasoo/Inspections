using Microsoft.EntityFrameworkCore.Migrations;

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class add_new_opre_fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "EarthFaultEFEnabled",
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
                name: "EarthFaultEIREnabled",
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
                name: "EarthFaultRoobEnabled",
                table: "OperationalReadings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<short>(
                name: "MainBreakerRating",
                table: "OperationalReadings",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

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
                name: "OverCurrentDTLEnabled",
                table: "OperationalReadings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OverCurrentDirectActing",
                table: "OperationalReadings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OverCurrentDirectActingEnabled",
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

            migrationBuilder.AddColumn<bool>(
                name: "OverCurrentIDTMLEnabled",
                table: "OperationalReadings",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EarthFaultEF",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "EarthFaultEFAt",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "EarthFaultEFEnabled",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "EarthFaultEIR",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "EarthFaultEIRAt",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "EarthFaultEIREnabled",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "EarthFaultRoob",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "EarthFaultRoobEnabled",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "MainBreakerRating",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "OverCurrentDTL",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "OverCurrentDTLAt",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "OverCurrentDTLEnabled",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "OverCurrentDirectActing",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "OverCurrentDirectActingEnabled",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "OverCurrentIDTML",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "OverCurrentIDTMLAt",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "OverCurrentIDTMLEnabled",
                table: "OperationalReadings");
        }
    }
}
