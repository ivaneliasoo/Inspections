using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class costsheet_final_overall_markup_field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "finalOverallMarkup",
                table: "CSTemplate",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "finalOverallMarkup",
                table: "CostSheet",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "finalOverallMarkup",
                table: "CSTemplate");

            migrationBuilder.DropColumn(
                name: "finalOverallMarkup",
                table: "CostSheet");
        }
    }
}
