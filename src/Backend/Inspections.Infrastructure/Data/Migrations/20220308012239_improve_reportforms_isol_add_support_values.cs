using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class improve_reportforms_isol_add_support_values : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormDefinitionReport");

            migrationBuilder.CreateTable(
                name: "ReportFroms",
                schema: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Values = table.Column<JsonDocument>(type: "jsonb", nullable: true),
                    LastEdit = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastEditUser = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ReportId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportFroms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportFroms_Reports_ReportId1",
                        column: x => x.ReportId1,
                        principalSchema: "Inspections",
                        principalTable: "Reports",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportFroms_ReportId1",
                schema: "Inspections",
                table: "ReportFroms",
                column: "ReportId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportFroms",
                schema: "Inspections");

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
                        name: "FK_FormDefinitionReport_FormsDefinitions_FormsId",
                        column: x => x.FormsId,
                        principalTable: "FormsDefinitions",
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

            migrationBuilder.CreateIndex(
                name: "IX_FormDefinitionReport_ReportsId",
                table: "FormDefinitionReport",
                column: "ReportsId");
        }
    }
}
