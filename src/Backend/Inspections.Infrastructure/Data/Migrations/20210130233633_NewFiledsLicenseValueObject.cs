using Microsoft.EntityFrameworkCore.Migrations;

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class NewFiledsLicenseValueObject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "License_Amp",
                schema: "Inspections",
                table: "Reports",
                nullable: true,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "License_Kva",
                schema: "Inspections",
                table: "Reports",
                nullable: true,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "License_Volt",
                schema: "Inspections",
                table: "Reports",
                nullable: true,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "License_Amp",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "License_Kva",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "License_Volt",
                schema: "Inspections",
                table: "Reports");
        }
    }
}
