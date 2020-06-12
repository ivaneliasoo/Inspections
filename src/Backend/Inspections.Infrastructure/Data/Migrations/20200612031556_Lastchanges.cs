using Microsoft.EntityFrameworkCore.Migrations;

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class Lastchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CheckListParams",
                schema: "Inspections",
                table: "CheckListParams");

            migrationBuilder.AlterColumn<int>(
                name: "CheckListItemId",
                schema: "Inspections",
                table: "CheckListParams",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CheckListId",
                schema: "Inspections",
                table: "CheckListParams",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CheckListParams",
                schema: "Inspections",
                table: "CheckListParams",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CheckListParams_CheckListId",
                schema: "Inspections",
                table: "CheckListParams",
                column: "CheckListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CheckListParams",
                schema: "Inspections",
                table: "CheckListParams");

            migrationBuilder.DropIndex(
                name: "IX_CheckListParams_CheckListId",
                schema: "Inspections",
                table: "CheckListParams");

            migrationBuilder.AlterColumn<int>(
                name: "CheckListItemId",
                schema: "Inspections",
                table: "CheckListParams",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CheckListId",
                schema: "Inspections",
                table: "CheckListParams",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CheckListParams",
                schema: "Inspections",
                table: "CheckListParams",
                columns: new[] { "CheckListId", "CheckListItemId" });
        }
    }
}
