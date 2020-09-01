using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class Address_Objects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                schema: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressLine = table.Column<string>(nullable: false),
                    AddressLine2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: false),
                    Province = table.Column<string>(nullable: false),
                    LastEdit = table.Column<DateTimeOffset>(nullable: false),
                    LastEditUser = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses",
                schema: "Inspections");
        }
    }
}
