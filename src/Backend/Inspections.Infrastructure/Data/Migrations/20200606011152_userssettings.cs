using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class userssettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastEditUser",
                schema: "Inspections",
                table: "Signatures",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastEditUser",
                schema: "Inspections",
                table: "ReportsConfiguration",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastEditUser",
                schema: "Inspections",
                table: "Reports",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastEditUser",
                schema: "Inspections",
                table: "Photos",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastEditUser",
                schema: "Inspections",
                table: "Notes",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastEditUser",
                schema: "Inspections",
                table: "CheckLists",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastEditUser",
                schema: "Inspections",
                table: "CheckListParams",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastEditUser",
                schema: "Inspections",
                table: "CheckListItems",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserName = table.Column<string>(maxLength: 20, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    LastEdit = table.Column<DateTimeOffset>(nullable: false),
                    LastEditUser = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserName);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropColumn(
                name: "LastEditUser",
                schema: "Inspections",
                table: "Signatures");

            migrationBuilder.DropColumn(
                name: "LastEditUser",
                schema: "Inspections",
                table: "ReportsConfiguration");

            migrationBuilder.DropColumn(
                name: "LastEditUser",
                schema: "Inspections",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "LastEditUser",
                schema: "Inspections",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "LastEditUser",
                schema: "Inspections",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "LastEditUser",
                schema: "Inspections",
                table: "CheckLists");

            migrationBuilder.DropColumn(
                name: "LastEditUser",
                schema: "Inspections",
                table: "CheckListParams");

            migrationBuilder.DropColumn(
                name: "LastEditUser",
                schema: "Inspections",
                table: "CheckListItems");
        }
    }
}
