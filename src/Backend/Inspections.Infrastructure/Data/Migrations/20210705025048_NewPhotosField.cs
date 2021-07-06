using Microsoft.EntityFrameworkCore.Migrations;

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class NewPhotosField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoStorageId",
                schema: "Inspections",
                table: "Photos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                schema: "Inspections",
                table: "Photos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailStorageId",
                schema: "Inspections",
                table: "Photos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                schema: "Inspections",
                table: "Photos",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoStorageId",
                schema: "Inspections",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                schema: "Inspections",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "ThumbnailStorageId",
                schema: "Inspections",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                schema: "Inspections",
                table: "Photos");
        }
    }
}
