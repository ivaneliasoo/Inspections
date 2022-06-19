using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class photorecord_in_report : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NeedsPhotoRecord",
                schema: "Inspections",
                table: "Reports",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeedsPhotoRecord",
                schema: "Inspections",
                table: "Reports");
        }
    }
}
