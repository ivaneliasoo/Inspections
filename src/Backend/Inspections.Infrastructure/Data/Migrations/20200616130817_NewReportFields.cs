using Microsoft.EntityFrameworkCore.Migrations;

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class NewReportFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FormName",
                schema: "Inspections",
                table: "Reports",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                schema: "Inspections",
                table: "Reports",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RemarksLabelText",
                schema: "Inspections",
                table: "Reports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "Inspections",
                table: "Reports",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NeedsCheck",
                schema: "Inspections",
                table: "Notes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormName",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "IsClosed",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "RemarksLabelText",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "NeedsCheck",
                schema: "Inspections",
                table: "Notes");
        }
    }
}
