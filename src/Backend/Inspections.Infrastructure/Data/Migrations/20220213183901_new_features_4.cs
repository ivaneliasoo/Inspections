using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class new_features_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OperationalReadings2",
                schema: "Inspections",
                table: "ReportsConfiguration",
                newName: "OperationalReadings");

            migrationBuilder.RenameColumn(
                name: "AdditionalFields2",
                schema: "Inspections",
                table: "ReportsConfiguration",
                newName: "AdditionalFields");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OperationalReadings",
                schema: "Inspections",
                table: "ReportsConfiguration",
                newName: "OperationalReadings2");

            migrationBuilder.RenameColumn(
                name: "AdditionalFields",
                schema: "Inspections",
                table: "ReportsConfiguration",
                newName: "AdditionalFields2");
        }
    }
}
