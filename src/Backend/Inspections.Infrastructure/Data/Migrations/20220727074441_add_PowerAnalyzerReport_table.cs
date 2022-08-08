using System;
using Inspections.Core.Domain;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class add_PowerAnalyzerReport_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PowerAnalyzerReport",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    location = table.Column<string>(type: "text", nullable: false),
                    template = table.Column<int>(type: "integer", nullable: false),
                    fileName = table.Column<string>(type: "text", nullable: false),
                    circuit = table.Column<string>(type: "text", nullable: true),
                    dateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    chartLegendOption = table.Column<string>(type: "text", nullable: true),
                    cover = table.Column<ReportCover>(type: "jsonb", nullable: true),
                    rawCsvData = table.Column<string>(type: "text", nullable: true),
                    lastUpdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<bool>(type: "boolean", nullable: false),
                    LastEdit = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastEditUser = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerAnalyzerReport", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PowerAnalyzerReport");
        }
    }
}
