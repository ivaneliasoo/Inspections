using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class dynamic_fields_generation_feature2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormDefinitionReport_FormDefinition_FormsId",
                table: "FormDefinitionReport");

            migrationBuilder.DropForeignKey(
                name: "FK_FormDefinitionReportConfiguration_FormDefinition_FormsId",
                table: "FormDefinitionReportConfiguration");

            migrationBuilder.DropTable(
                name: "OperationalReadings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormDefinition",
                table: "FormDefinition");

            migrationBuilder.RenameTable(
                name: "FormDefinition",
                newName: "FormsDefinitions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormsDefinitions",
                table: "FormsDefinitions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FormDefinitionReport_FormsDefinitions_FormsId",
                table: "FormDefinitionReport",
                column: "FormsId",
                principalTable: "FormsDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FormDefinitionReportConfiguration_FormsDefinitions_FormsId",
                table: "FormDefinitionReportConfiguration",
                column: "FormsId",
                principalTable: "FormsDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormDefinitionReport_FormsDefinitions_FormsId",
                table: "FormDefinitionReport");

            migrationBuilder.DropForeignKey(
                name: "FK_FormDefinitionReportConfiguration_FormsDefinitions_FormsId",
                table: "FormDefinitionReportConfiguration");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FormsDefinitions",
                table: "FormsDefinitions");

            migrationBuilder.RenameTable(
                name: "FormsDefinitions",
                newName: "FormDefinition");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FormDefinition",
                table: "FormDefinition",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "OperationalReadings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AdditionalFields = table.Column<string>(type: "text", nullable: true),
                    EarthFaultA = table.Column<short>(type: "smallint", nullable: false),
                    EarthFaultEFEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    EarthFaultEIREnabled = table.Column<bool>(type: "boolean", nullable: false),
                    EarthFaultELRA = table.Column<short>(type: "smallint", nullable: false),
                    EarthFaultELRSec = table.Column<short>(type: "smallint", nullable: false),
                    EarthFaultMA = table.Column<short>(type: "smallint", nullable: false),
                    EarthFaultRoobEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    EarthFaultSec = table.Column<short>(type: "smallint", nullable: false),
                    LastEdit = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastEditUser = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    MainBreakerAmp = table.Column<short>(type: "smallint", nullable: false),
                    MainBreakerCapacity = table.Column<short>(type: "smallint", nullable: false),
                    MainBreakerPoles = table.Column<short>(type: "smallint", nullable: false),
                    MainBreakerRating = table.Column<short>(type: "smallint", nullable: false),
                    OverCurrentDTLA = table.Column<short>(type: "smallint", nullable: false),
                    OverCurrentDTLEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    OverCurrentDTLSec = table.Column<short>(type: "smallint", nullable: false),
                    OverCurrentDirectActing = table.Column<short>(type: "smallint", nullable: false),
                    OverCurrentDirectActingEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    OverCurrentIDMTLA = table.Column<short>(type: "smallint", nullable: false),
                    OverCurrentIDMTLTm = table.Column<short>(type: "smallint", nullable: false),
                    OverCurrentIDTMLEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    RunningLoadL1 = table.Column<short>(type: "smallint", nullable: false),
                    RunningLoadL2 = table.Column<short>(type: "smallint", nullable: false),
                    RunningLoadL3 = table.Column<short>(type: "smallint", nullable: false),
                    VoltageL1L2 = table.Column<short>(type: "smallint", nullable: false),
                    VoltageL1L3 = table.Column<short>(type: "smallint", nullable: false),
                    VoltageL1N = table.Column<short>(type: "smallint", nullable: false),
                    VoltageL2L3 = table.Column<short>(type: "smallint", nullable: false),
                    VoltageL2N = table.Column<short>(type: "smallint", nullable: false),
                    VoltageL3N = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationalReadings", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_FormDefinitionReport_FormDefinition_FormsId",
                table: "FormDefinitionReport",
                column: "FormsId",
                principalTable: "FormDefinition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FormDefinitionReportConfiguration_FormDefinition_FormsId",
                table: "FormDefinitionReportConfiguration",
                column: "FormsId",
                principalTable: "FormDefinition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
