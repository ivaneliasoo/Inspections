using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class photorecord_user_sugnature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Signature",
                table: "Users",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Signature",
                table: "Users");
        }
    }
}
