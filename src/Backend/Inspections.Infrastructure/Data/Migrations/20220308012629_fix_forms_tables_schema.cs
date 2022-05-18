using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class fix_forms_tables_schema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormDefinitionReportConfiguration_FormsDefinitions_FormsId",
                table: "FormDefinitionReportConfiguration");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormsDefinitions",
                table: "FormsDefinitions");

            migrationBuilder.RenameTable(
                name: "FormDefinitionReportConfiguration",
                newName: "FormDefinitionReportConfiguration",
                newSchema: "Inspections");

            migrationBuilder.RenameTable(
                name: "FormsDefinitions",
                newName: "FormDefinition",
                newSchema: "Inspections");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormDefinition",
                schema: "Inspections",
                table: "FormDefinition",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FormDefinitionReportConfiguration_FormDefinition_FormsId",
                schema: "Inspections",
                table: "FormDefinitionReportConfiguration",
                column: "FormsId",
                principalSchema: "Inspections",
                principalTable: "FormDefinition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormDefinitionReportConfiguration_FormDefinition_FormsId",
                schema: "Inspections",
                table: "FormDefinitionReportConfiguration");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormDefinition",
                schema: "Inspections",
                table: "FormDefinition");

            migrationBuilder.RenameTable(
                name: "FormDefinitionReportConfiguration",
                schema: "Inspections",
                newName: "FormDefinitionReportConfiguration");

            migrationBuilder.RenameTable(
                name: "FormDefinition",
                schema: "Inspections",
                newName: "FormsDefinitions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormsDefinitions",
                table: "FormsDefinitions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FormDefinitionReportConfiguration_FormsDefinitions_FormsId",
                table: "FormDefinitionReportConfiguration",
                column: "FormsId",
                principalTable: "FormsDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
