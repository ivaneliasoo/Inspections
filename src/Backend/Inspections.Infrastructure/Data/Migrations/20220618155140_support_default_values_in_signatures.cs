using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class support_default_values_in_signatures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultResponsibleType",
                schema: "Inspections",
                table: "Signatures",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseLoggedInUserAsDefault",
                schema: "Inspections",
                table: "Signatures",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultResponsibleType",
                schema: "Inspections",
                table: "Signatures");

            migrationBuilder.DropColumn(
                name: "UseLoggedInUserAsDefault",
                schema: "Inspections",
                table: "Signatures");
        }
    }
}
