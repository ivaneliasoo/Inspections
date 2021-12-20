using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class options : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "excludeSunday",
                table: "SchedJob",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalFileds",
                schema: "Inspections",
                table: "ReportsConfiguration",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalFields",
                table: "OperationalReadings",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Options",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    scheduleWeeks = table.Column<int>(type: "integer", nullable: false),
                    autosaveInterval = table.Column<int>(type: "integer", nullable: false),
                    lastUpdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastEdit = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastEditUser = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Options");

            migrationBuilder.DropColumn(
                name: "excludeSunday",
                table: "SchedJob");

            migrationBuilder.DropColumn(
                name: "AdditionalFileds",
                schema: "Inspections",
                table: "ReportsConfiguration");

            migrationBuilder.DropColumn(
                name: "AdditionalFields",
                table: "OperationalReadings");
        }
    }
}
