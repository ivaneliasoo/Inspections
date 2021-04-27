using Microsoft.EntityFrameworkCore.Migrations;

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class SignaturesOrdering : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "Order",
                schema: "Inspections",
                table: "Signatures",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                schema: "Inspections",
                table: "Signatures");
        }
    }
}
