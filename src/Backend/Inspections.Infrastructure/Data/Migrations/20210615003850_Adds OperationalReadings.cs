using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class AddsOperationalReadings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OperationalReadings",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.AddColumn<int>(
                name: "OperationalReadingsId",
                schema: "Inspections",
                table: "Reports",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OperationalReadings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VoltageL1N = table.Column<short>(type: "smallint", nullable: false),
                    VoltageL2N = table.Column<short>(type: "smallint", nullable: false),
                    VoltageL3N = table.Column<short>(type: "smallint", nullable: false),
                    VoltageL1L2 = table.Column<short>(type: "smallint", nullable: false),
                    VoltageL1L3 = table.Column<short>(type: "smallint", nullable: false),
                    VoltageL2L3 = table.Column<short>(type: "smallint", nullable: false),
                    RunningLoadL1 = table.Column<short>(type: "smallint", nullable: false),
                    RunningLoadL2 = table.Column<short>(type: "smallint", nullable: false),
                    RunningLoadL3 = table.Column<short>(type: "smallint", nullable: false),
                    MainBreakerAmp = table.Column<short>(type: "smallint", nullable: false),
                    MainBreakerPoles = table.Column<short>(type: "smallint", nullable: false),
                    MainBreakerCapacity = table.Column<short>(type: "smallint", nullable: false),
                    OverCurrentByMainBreaker = table.Column<bool>(type: "boolean", nullable: false),
                    OverCurrentDTLA = table.Column<short>(type: "smallint", nullable: false),
                    OverCurrentDTLSec = table.Column<short>(type: "smallint", nullable: false),
                    OverCurrentIDMTLA = table.Column<short>(type: "smallint", nullable: false),
                    OverCurrentIDMTLTm = table.Column<short>(type: "smallint", nullable: false),
                    EarthFaultMA = table.Column<short>(type: "smallint", nullable: false),
                    EarthFaultELRA = table.Column<short>(type: "smallint", nullable: false),
                    EarthFaultELRSec = table.Column<short>(type: "smallint", nullable: false),
                    EarthFaultA = table.Column<short>(type: "smallint", nullable: false),
                    EarthFaultSec = table.Column<short>(type: "smallint", nullable: false),
                    LastEdit = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastEditUser = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationalReadings", x => x.Id);
                });

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
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_OperationalReadings_OperationalReadingsId",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.DropTable(
                name: "OperationalReadings");

            migrationBuilder.DropIndex(
                name: "IX_Reports_OperationalReadingsId",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "OperationalReadingsId",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.AddColumn<string>(
                name: "OperationalReadings",
                schema: "Inspections",
                table: "Reports",
                type: "text",
                nullable: true);
        }
    }
}
