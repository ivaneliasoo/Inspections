using Microsoft.EntityFrameworkCore.Migrations;

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class NewLIcenseFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amp",
                table: "Licenses",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Contact",
                table: "Licenses",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Licenses",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "KVA",
                table: "Licenses",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Licenses",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PersonInCharge",
                table: "Licenses",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Volt",
                table: "Licenses",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amp",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "Contact",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "KVA",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "PersonInCharge",
                table: "Licenses");

            migrationBuilder.DropColumn(
                name: "Volt",
                table: "Licenses");
        }
    }
}
