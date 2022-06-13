using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class checklists_not_required_license_forAddress_not_required : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAddressRequired",
                schema: "Inspections",
                table: "ReportsConfiguration");

            migrationBuilder.DropColumn(
                name: "IsLicenseRequired",
                schema: "Inspections",
                table: "ReportsConfiguration");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAddressRequired",
                schema: "Inspections",
                table: "ReportsConfiguration",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLicenseRequired",
                schema: "Inspections",
                table: "ReportsConfiguration",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
