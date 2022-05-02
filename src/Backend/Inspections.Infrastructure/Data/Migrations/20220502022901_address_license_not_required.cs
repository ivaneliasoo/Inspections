using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class address_license_not_required : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Licenses_LicenseId",
                schema: "Inspections",
                table: "Addresses");

            migrationBuilder.AlterColumn<int>(
                name: "LicenseId",
                schema: "Inspections",
                table: "Addresses",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Licenses_LicenseId",
                schema: "Inspections",
                table: "Addresses",
                column: "LicenseId",
                principalSchema: "Inspections",
                principalTable: "Licenses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Licenses_LicenseId",
                schema: "Inspections",
                table: "Addresses");

            migrationBuilder.AlterColumn<int>(
                name: "LicenseId",
                schema: "Inspections",
                table: "Addresses",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Licenses_LicenseId",
                schema: "Inspections",
                table: "Addresses",
                column: "LicenseId",
                principalSchema: "Inspections",
                principalTable: "Licenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
