using System;
using System.Text.Json;
using Inspections.Core.Domain.ReportConfigurationAggregate;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class dynamic_fields_generation_feature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_OperationalReadings_OperationalReadingsId",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_OperationalReadingsId",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "AdditionalFields",
                schema: "Inspections",
                table: "ReportsConfiguration");

            migrationBuilder.DropColumn(
                name: "OperationalReadings",
                schema: "Inspections",
                table: "ReportsConfiguration");

            migrationBuilder.DropColumn(
                name: "DynamicFields",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "DynamicOperationalReadings",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "OperationalReadingsId",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.CreateTable(
                name: "FormDefinition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Fields = table.Column<DynamicFields>(type: "jsonb", nullable: false, defaultValueSql: "'{ \"FieldsDefinitions\": null }'::jsonb"),
                    DefaultValues = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    LastEdit = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastEditUser = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormDefinition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormDefinitionReport",
                columns: table => new
                {
                    FormsId = table.Column<int>(type: "integer", nullable: false),
                    ReportsId = table.Column<int>(type: "integer", nullable: false),
                    LastEdit = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastEditUser = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormDefinitionReport", x => new { x.FormsId, x.ReportsId });
                    table.ForeignKey(
                        name: "FK_FormDefinitionReport_FormDefinition_FormsId",
                        column: x => x.FormsId,
                        principalTable: "FormDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormDefinitionReport_Reports_ReportsId",
                        column: x => x.ReportsId,
                        principalSchema: "Inspections",
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FormDefinitionReportConfiguration",
                columns: table => new
                {
                    FormsId = table.Column<int>(type: "integer", nullable: false),
                    ReportConfigurationsId = table.Column<int>(type: "integer", nullable: false),
                    LastEdit = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastEditUser = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormDefinitionReportConfiguration", x => new { x.FormsId, x.ReportConfigurationsId });
                    table.ForeignKey(
                        name: "FK_FormDefinitionReportConfiguration_FormDefinition_FormsId",
                        column: x => x.FormsId,
                        principalTable: "FormDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormDefinitionReportConfiguration_ReportsConfiguration_Repo~",
                        column: x => x.ReportConfigurationsId,
                        principalSchema: "Inspections",
                        principalTable: "ReportsConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormDefinitionReport_ReportsId",
                table: "FormDefinitionReport",
                column: "ReportsId");

            migrationBuilder.CreateIndex(
                name: "IX_FormDefinitionReportConfiguration_ReportConfigurationsId",
                table: "FormDefinitionReportConfiguration",
                column: "ReportConfigurationsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormDefinitionReport");

            migrationBuilder.DropTable(
                name: "FormDefinitionReportConfiguration");

            migrationBuilder.DropTable(
                name: "FormDefinition");

            migrationBuilder.AddColumn<DynamicFields>(
                name: "AdditionalFields",
                schema: "Inspections",
                table: "ReportsConfiguration",
                type: "jsonb",
                nullable: true,
                defaultValueSql: "'{ \"FieldsDefinitions\": null }'::jsonb");

            migrationBuilder.AddColumn<DynamicFields>(
                name: "OperationalReadings",
                schema: "Inspections",
                table: "ReportsConfiguration",
                type: "jsonb",
                nullable: true,
                defaultValueSql: "'{ \"FieldsDefinitions\": null }'::jsonb");

            migrationBuilder.AddColumn<string>(
                name: "DynamicFields",
                schema: "Inspections",
                table: "Reports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DynamicOperationalReadings",
                schema: "Inspections",
                table: "Reports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OperationalReadingsId",
                schema: "Inspections",
                table: "Reports",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_OperationalReadingsId",
                schema: "Inspections",
                table: "Reports",
                column: "OperationalReadingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_OperationalReadings_OperationalReadingsId",
                schema: "Inspections",
                table: "Reports",
                column: "OperationalReadingsId",
                principalTable: "OperationalReadings",
                principalColumn: "Id");
        }
    }
}
