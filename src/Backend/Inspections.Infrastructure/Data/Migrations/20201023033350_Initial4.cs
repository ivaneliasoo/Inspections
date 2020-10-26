using Microsoft.EntityFrameworkCore.Migrations;

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class Initial4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Addresses_LicenseId",
                schema: "Inspections",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_LicenseId",
                schema: "Inspections",
                table: "Addresses",
                column: "LicenseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Addresses_LicenseId",
                schema: "Inspections",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_LicenseId",
                schema: "Inspections",
                table: "Addresses",
                column: "LicenseId",
                unique: true);
        }
    }
}
