using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class ChangeLicensePersistence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "License_Number",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "License_Validity_End",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "License_Validity_Start",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "License_Volt",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.AddColumn<int>(
                name: "EMALicenseId",
                schema: "Inspections",
                table: "Reports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_EMALicenseId",
                schema: "Inspections",
                table: "Reports",
                column: "EMALicenseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Licenses_EMALicenseId",
                schema: "Inspections",
                table: "Reports",
                column: "EMALicenseId",
                principalTable: "Licenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Licenses_EMALicenseId",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_EMALicenseId",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "EMALicenseId",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.AddColumn<decimal>(
                name: "License_Amp",
                schema: "Inspections",
                table: "Reports",
                type: "numeric",
                nullable: true,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "License_Kva",
                schema: "Inspections",
                table: "Reports",
                type: "numeric",
                nullable: true,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "License_Number",
                schema: "Inspections",
                table: "Reports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "License_Validity_End",
                schema: "Inspections",
                table: "Reports",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "License_Validity_Start",
                schema: "Inspections",
                table: "Reports",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "License_Volt",
                schema: "Inspections",
                table: "Reports",
                type: "numeric",
                nullable: true,
                defaultValue: 0m);
        }
    }
}
