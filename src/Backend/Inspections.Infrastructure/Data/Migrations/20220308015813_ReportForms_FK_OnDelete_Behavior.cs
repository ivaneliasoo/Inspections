using Inspections.Core.Domain.ReportConfigurationAggregate;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class ReportForms_FK_OnDelete_Behavior : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportFroms_Reports_ReportId1",
                schema: "Inspections",
                table: "ReportFroms");

            migrationBuilder.DropIndex(
                name: "IX_ReportFroms_ReportId1",
                schema: "Inspections",
                table: "ReportFroms");

            migrationBuilder.DropColumn(
                name: "ReportId1",
                schema: "Inspections",
                table: "ReportFroms");

            migrationBuilder.RenameTable(
                name: "Licenses",
                newName: "Licenses",
                newSchema: "Inspections");

            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                schema: "Inspections",
                table: "ReportFroms",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DynamicFields>(
                name: "Fields",
                schema: "Inspections",
                table: "ReportFroms",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{ \"FieldsDefinitions\": null }'::jsonb");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                schema: "Inspections",
                table: "ReportFroms",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Inspections",
                table: "ReportFroms",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                schema: "Inspections",
                table: "ReportFroms",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "Inspections",
                table: "ReportFroms",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ReportFroms_ReportId",
                schema: "Inspections",
                table: "ReportFroms",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportFroms_Reports_ReportId",
                schema: "Inspections",
                table: "ReportFroms",
                column: "ReportId",
                principalSchema: "Inspections",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportFroms_Reports_ReportId",
                schema: "Inspections",
                table: "ReportFroms");

            migrationBuilder.DropIndex(
                name: "IX_ReportFroms_ReportId",
                schema: "Inspections",
                table: "ReportFroms");

            migrationBuilder.DropColumn(
                name: "Enabled",
                schema: "Inspections",
                table: "ReportFroms");

            migrationBuilder.DropColumn(
                name: "Fields",
                schema: "Inspections",
                table: "ReportFroms");

            migrationBuilder.DropColumn(
                name: "Icon",
                schema: "Inspections",
                table: "ReportFroms");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Inspections",
                table: "ReportFroms");

            migrationBuilder.DropColumn(
                name: "ReportId",
                schema: "Inspections",
                table: "ReportFroms");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "Inspections",
                table: "ReportFroms");

            migrationBuilder.RenameTable(
                name: "Licenses",
                schema: "Inspections",
                newName: "Licenses");

            migrationBuilder.AddColumn<int>(
                name: "ReportId1",
                schema: "Inspections",
                table: "ReportFroms",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReportFroms_ReportId1",
                schema: "Inspections",
                table: "ReportFroms",
                column: "ReportId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportFroms_Reports_ReportId1",
                schema: "Inspections",
                table: "ReportFroms",
                column: "ReportId1",
                principalSchema: "Inspections",
                principalTable: "Reports",
                principalColumn: "Id");
        }
    }
}
