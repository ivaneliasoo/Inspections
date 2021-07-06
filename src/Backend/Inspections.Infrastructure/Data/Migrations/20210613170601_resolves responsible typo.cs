using Microsoft.EntityFrameworkCore.Migrations;

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class resolvesresponsibletypo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Responsable_Type",
                schema: "Inspections",
                table: "Signatures",
                newName: "Responsible_Type");

            migrationBuilder.RenameColumn(
                name: "Responsable_Name",
                schema: "Inspections",
                table: "Signatures",
                newName: "Responsible_Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Responsible_Type",
                schema: "Inspections",
                table: "Signatures",
                newName: "Responsable_Type");

            migrationBuilder.RenameColumn(
                name: "Responsible_Name",
                schema: "Inspections",
                table: "Signatures",
                newName: "Responsable_Name");
        }
    }
}
