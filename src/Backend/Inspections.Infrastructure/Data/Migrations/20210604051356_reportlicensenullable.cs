using Microsoft.EntityFrameworkCore.Migrations;

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class reportlicensenullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Licenses_EMALicenseId",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.AlterColumn<int>(
                name: "EMALicenseId",
                schema: "Inspections",
                table: "Reports",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Licenses_EMALicenseId",
                schema: "Inspections",
                table: "Reports",
                column: "EMALicenseId",
                principalTable: "Licenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Licenses_EMALicenseId",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.AlterColumn<int>(
                name: "EMALicenseId",
                schema: "Inspections",
                table: "Reports",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Licenses_EMALicenseId",
                schema: "Inspections",
                table: "Reports",
                column: "EMALicenseId",
                principalTable: "Licenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
