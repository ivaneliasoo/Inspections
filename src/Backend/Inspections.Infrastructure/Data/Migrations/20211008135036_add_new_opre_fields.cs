using Microsoft.EntityFrameworkCore.Migrations;

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class add_new_opre_fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OverCurrentByMainBreaker",
                table: "OperationalReadings",
                newName: "OverCurrentIDTMLEnabled");

            migrationBuilder.AddColumn<bool>(
                name: "EarthFaultEFEnabled",
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
                name: "OverCurrentDTLEnabled",
                table: "OperationalReadings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<short>(
                name: "OverCurrentDirectActing",
                table: "OperationalReadings",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<bool>(
                name: "OverCurrentDirectActingEnabled",
                table: "OperationalReadings",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EarthFaultEFEnabled",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "EarthFaultEIREnabled",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "EarthFaultRoobEnabled",
                table: "OperationalReadings");

            migrationBuilder.DropColumn(
                name: "MainBreakerRating",
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

            migrationBuilder.RenameColumn(
                name: "OverCurrentIDTMLEnabled",
                table: "OperationalReadings",
                newName: "OverCurrentByMainBreaker");
        }
    }
}
