using Microsoft.EntityFrameworkCore.Migrations;

namespace Inspections.Infrastructure.data.Migrations
{
    public partial class Changephotorecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                schema: "Inspections",
                table: "Photos");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                schema: "Inspections",
                table: "Photos",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                schema: "Inspections",
                table: "Photos");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                schema: "Inspections",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
