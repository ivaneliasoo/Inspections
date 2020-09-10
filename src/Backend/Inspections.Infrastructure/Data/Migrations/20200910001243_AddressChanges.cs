using Microsoft.EntityFrameworkCore.Migrations;

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class AddressChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                schema: "Inspections",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Province",
                schema: "Inspections",
                table: "Addresses");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                schema: "Inspections",
                table: "Addresses",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                schema: "Inspections",
                table: "Addresses",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                schema: "Inspections",
                table: "Addresses",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                schema: "Inspections",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                schema: "Inspections",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Unit",
                schema: "Inspections",
                table: "Addresses");

            migrationBuilder.AddColumn<string>(
                name: "City",
                schema: "Inspections",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Province",
                schema: "Inspections",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
