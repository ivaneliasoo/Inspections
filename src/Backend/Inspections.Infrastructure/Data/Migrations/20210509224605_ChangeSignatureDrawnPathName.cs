using Microsoft.EntityFrameworkCore.Migrations;

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class ChangeSignatureDrawnPathName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DrawedSign",
                schema: "Inspections",
                table: "Signatures",
                newName: "DrawnSign");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DrawnSign",
                schema: "Inspections",
                table: "Signatures",
                newName: "DrawedSign");
        }
    }
}
