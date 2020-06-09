using Microsoft.EntityFrameworkCore.Migrations;

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class addforeignfieldonparams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckListParams_CheckListItems_CheckListFK",
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
                name: "CheckListFK",
                schema: "Inspections",
                table: "CheckListParams",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckListParams_CheckListItems_CheckListFK",
                schema: "Inspections",
                table: "CheckListParams",
                column: "CheckListFK",
                principalSchema: "Inspections",
                principalTable: "CheckListItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckListParams_CheckListItems_CheckListFK",
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
                name: "CheckListFK",
                schema: "Inspections",
                table: "CheckListParams",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckListParams_CheckListItems_CheckListFK",
                schema: "Inspections",
                table: "CheckListParams",
                column: "CheckListFK",
                principalSchema: "Inspections",
                principalTable: "CheckListItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
