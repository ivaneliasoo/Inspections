using System;
using Inspections.Core.Domain;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class createnewcostsheettable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostSheet",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    project = table.Column<string>(type: "text", nullable: false),
                    location = table.Column<string>(type: "text", nullable: false),
                    dateCreated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    materialMarkup = table.Column<double>(type: "double precision", nullable: false),
                    labourDailyRate = table.Column<double>(type: "double precision", nullable: false),
                    labourNightMultiplier = table.Column<double>(type: "double precision", nullable: false),
                    finalMarkup = table.Column<double>(type: "double precision", nullable: false),
                    sections = table.Column<Section[]>(type: "jsonb", nullable: true),
                    lastUpdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated = table.Column<bool>(type: "boolean", nullable: false),
                    LastEdit = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastEditUser = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostSheet", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostSheet");
        }
    }
}
