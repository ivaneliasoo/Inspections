using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class add_base_url_field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                schema: "Inspections",
                table: "Photos");

            migrationBuilder.RenameColumn(
                name: "ThumbnailUrl",
                schema: "Inspections",
                table: "Photos",
                newName: "BaseURL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BaseURL",
                schema: "Inspections",
                table: "Photos",
                newName: "ThumbnailUrl");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                schema: "Inspections",
                table: "Photos",
                type: "text",
                nullable: true);
        }
    }
}
