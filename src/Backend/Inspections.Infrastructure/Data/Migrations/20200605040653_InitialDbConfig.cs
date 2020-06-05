using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class InitialDbConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Inspections");

            migrationBuilder.CreateTable(
                name: "Notes",
                schema: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: false),
                    Checked = table.Column<bool>(nullable: false),
                    ShouldCheck = table.Column<bool>(nullable: false),
                    IsConfiguration = table.Column<bool>(nullable: false),
                    LastEdit = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                schema: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(nullable: false),
                    FilePath = table.Column<string>(nullable: false),
                    Label = table.Column<string>(nullable: false),
                    LastEdit = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                schema: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    License_LicenseType = table.Column<int>(nullable: true),
                    License_Number = table.Column<string>(nullable: true),
                    License_Validity_Start = table.Column<DateTime>(nullable: true),
                    License_Validity_End = table.Column<DateTime>(nullable: true),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    LastEdit = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportsConfiguration",
                schema: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    FormName = table.Column<string>(nullable: false),
                    RemarksLabelText = table.Column<string>(nullable: true),
                    LastEdit = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportsConfiguration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CheckLists",
                schema: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(nullable: false),
                    Annotation = table.Column<string>(nullable: true),
                    IsConfiguration = table.Column<bool>(nullable: false),
                    LastEdit = table.Column<DateTimeOffset>(nullable: false),
                    ReportConfigurationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckLists_ReportsConfiguration_ReportConfigurationId",
                        column: x => x.ReportConfigurationId,
                        principalSchema: "Inspections",
                        principalTable: "ReportsConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Signatures",
                schema: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: false),
                    Annotation = table.Column<string>(nullable: true),
                    Responsable_Type = table.Column<int>(nullable: true),
                    Responsable_Name = table.Column<string>(nullable: true),
                    Designation = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    Principal = table.Column<bool>(nullable: false),
                    IsConfiguration = table.Column<bool>(nullable: false),
                    LastEdit = table.Column<DateTimeOffset>(nullable: false),
                    ReportConfigurationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Signatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Signatures_ReportsConfiguration_ReportConfigurationId",
                        column: x => x.ReportConfigurationId,
                        principalSchema: "Inspections",
                        principalTable: "ReportsConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CheckListItems",
                schema: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CheckListId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: false),
                    Checked = table.Column<int>(nullable: false),
                    Required = table.Column<bool>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    LastEdit = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckListItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckListItems_CheckLists_CheckListId",
                        column: x => x.CheckListId,
                        principalSchema: "Inspections",
                        principalTable: "CheckLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckListParams",
                schema: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CheckListItemId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    CheckListFK = table.Column<int>(nullable: false),
                    CheckListId = table.Column<int>(nullable: true),
                    LastEdit = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckListParams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckListParams_CheckListItems_CheckListFK",
                        column: x => x.CheckListFK,
                        principalSchema: "Inspections",
                        principalTable: "CheckListItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckListParams_CheckLists_CheckListId",
                        column: x => x.CheckListId,
                        principalSchema: "Inspections",
                        principalTable: "CheckLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckListItems_CheckListId",
                schema: "Inspections",
                table: "CheckListItems",
                column: "CheckListId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckListParams_CheckListFK",
                schema: "Inspections",
                table: "CheckListParams",
                column: "CheckListFK");

            migrationBuilder.CreateIndex(
                name: "IX_CheckListParams_CheckListId",
                schema: "Inspections",
                table: "CheckListParams",
                column: "CheckListId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckLists_ReportConfigurationId",
                schema: "Inspections",
                table: "CheckLists",
                column: "ReportConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_Signatures_ReportConfigurationId",
                schema: "Inspections",
                table: "Signatures",
                column: "ReportConfigurationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckListParams",
                schema: "Inspections");

            migrationBuilder.DropTable(
                name: "Notes",
                schema: "Inspections");

            migrationBuilder.DropTable(
                name: "Photos",
                schema: "Inspections");

            migrationBuilder.DropTable(
                name: "Reports",
                schema: "Inspections");

            migrationBuilder.DropTable(
                name: "Signatures",
                schema: "Inspections");

            migrationBuilder.DropTable(
                name: "CheckListItems",
                schema: "Inspections");

            migrationBuilder.DropTable(
                name: "CheckLists",
                schema: "Inspections");

            migrationBuilder.DropTable(
                name: "ReportsConfiguration",
                schema: "Inspections");
        }
    }
}
