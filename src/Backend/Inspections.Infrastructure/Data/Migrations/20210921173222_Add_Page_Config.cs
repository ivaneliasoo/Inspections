using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class Add_Page_Config : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "Footer",
                schema: "Inspections",
                table: "ReportsConfiguration",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MarginBottom",
                schema: "Inspections",
                table: "ReportsConfiguration",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MarginLeft",
                schema: "Inspections",
                table: "ReportsConfiguration",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MarginRight",
                schema: "Inspections",
                table: "ReportsConfiguration",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MarginTop",
                schema: "Inspections",
                table: "ReportsConfiguration",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "LastEditUser",
                schema: "Inspections",
                table: "Addresses",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastEdit",
                schema: "Inspections",
                table: "Addresses",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine2",
                schema: "Inspections",
                table: "Addresses",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine",
                schema: "Inspections",
                table: "Addresses",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "Inspections",
                table: "Addresses",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Footer",
                schema: "Inspections",
                table: "ReportsConfiguration");

            migrationBuilder.DropColumn(
                name: "MarginBottom",
                schema: "Inspections",
                table: "ReportsConfiguration");

            migrationBuilder.DropColumn(
                name: "MarginLeft",
                schema: "Inspections",
                table: "ReportsConfiguration");

            migrationBuilder.DropColumn(
                name: "MarginRight",
                schema: "Inspections",
                table: "ReportsConfiguration");

            migrationBuilder.DropColumn(
                name: "MarginTop",
                schema: "Inspections",
                table: "ReportsConfiguration");

            migrationBuilder.AlterColumn<string>(
                name: "LastEditUser",
                schema: "Inspections",
                table: "Addresses",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastEdit",
                schema: "Inspections",
                table: "Addresses",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine2",
                schema: "Inspections",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine",
                schema: "Inspections",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "Inspections",
                table: "Addresses",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

        }
    }
}
