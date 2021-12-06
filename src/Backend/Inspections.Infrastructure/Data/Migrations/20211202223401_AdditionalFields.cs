using Microsoft.EntityFrameworkCore.Migrations;

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class AdditionalFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "PrintSections",
                newName: "PrintSections",
                newSchema: "Inspections");

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalFileds",
                schema: "Inspections",
                table: "ReportsConfiguration",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalFields",
                table: "OperationalReadings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                schema: "Inspections",
                table: "PrintSections",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "IsMainReport",
                schema: "Inspections",
                table: "PrintSections",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "PrintSections",
                schema: "Inspections",
                newName: "PrintSections");

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalFileds",
                schema: "Inspections",
                table: "ReportsConfiguration",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalFields",
                table: "OperationalReadings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "PrintSections",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<bool>(
                name: "IsMainReport",
                table: "PrintSections",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);
        }
    }
}
