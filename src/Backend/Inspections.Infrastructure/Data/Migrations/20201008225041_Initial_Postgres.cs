using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Inspections.Infrastructure.Data.Migrations
{
    public partial class Initial_Postgres : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Inspections");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserName = table.Column<string>(maxLength: 20, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(nullable: true),
                    LastEditedReport = table.Column<int>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false),
                    LastEdit = table.Column<DateTimeOffset>(nullable: false),
                    LastEditUser = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                schema: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AddressLine = table.Column<string>(nullable: false),
                    AddressLine2 = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: false),
                    Country = table.Column<string>(nullable: false),
                    PostalCode = table.Column<string>(nullable: false),
                    LastEdit = table.Column<DateTimeOffset>(nullable: false),
                    LastEditUser = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                schema: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    License_LicenseType = table.Column<int>(nullable: true),
                    License_Number = table.Column<string>(nullable: true),
                    License_Validity_Start = table.Column<DateTime>(nullable: true),
                    License_Validity_End = table.Column<DateTime>(nullable: true),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    IsClosed = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    FormName = table.Column<string>(nullable: false),
                    RemarksLabelText = table.Column<string>(nullable: false),
                    LastEdit = table.Column<DateTimeOffset>(nullable: false),
                    LastEditUser = table.Column<string>(maxLength: 20, nullable: false)
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    FormName = table.Column<string>(nullable: false),
                    RemarksLabelText = table.Column<string>(nullable: true),
                    LastEdit = table.Column<DateTimeOffset>(nullable: false),
                    LastEditUser = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportsConfiguration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                schema: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReportId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: false),
                    Checked = table.Column<bool>(nullable: false),
                    NeedsCheck = table.Column<bool>(nullable: false),
                    LastEdit = table.Column<DateTimeOffset>(nullable: false),
                    LastEditUser = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_Reports_ReportId",
                        column: x => x.ReportId,
                        principalSchema: "Inspections",
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                schema: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReportId = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(nullable: false),
                    Label = table.Column<string>(nullable: false),
                    LastEdit = table.Column<DateTimeOffset>(nullable: false),
                    LastEditUser = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Reports_ReportId",
                        column: x => x.ReportId,
                        principalSchema: "Inspections",
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckLists",
                schema: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReportId = table.Column<int>(nullable: true),
                    ReportConfigurationId = table.Column<int>(nullable: true),
                    Text = table.Column<string>(nullable: false),
                    Annotation = table.Column<string>(nullable: true),
                    IsConfiguration = table.Column<bool>(nullable: false),
                    LastEdit = table.Column<DateTimeOffset>(nullable: false),
                    LastEditUser = table.Column<string>(maxLength: 20, nullable: false)
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
                    table.ForeignKey(
                        name: "FK_CheckLists_Reports_ReportId",
                        column: x => x.ReportId,
                        principalSchema: "Inspections",
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Signatures",
                schema: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(nullable: false),
                    Annotation = table.Column<string>(nullable: true),
                    Responsable_Type = table.Column<int>(nullable: true),
                    Responsable_Name = table.Column<string>(nullable: true),
                    Designation = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    Principal = table.Column<bool>(nullable: false),
                    IsConfiguration = table.Column<bool>(nullable: false),
                    ReportId = table.Column<int>(nullable: true),
                    ReportConfigurationId = table.Column<int>(nullable: true),
                    DrawedSign = table.Column<string>(nullable: true),
                    LastEdit = table.Column<DateTimeOffset>(nullable: false),
                    LastEditUser = table.Column<string>(maxLength: 20, nullable: false)
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
                    table.ForeignKey(
                        name: "FK_Signatures_Reports_ReportId",
                        column: x => x.ReportId,
                        principalSchema: "Inspections",
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CheckListItems",
                schema: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CheckListId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: false),
                    Checked = table.Column<int>(nullable: false),
                    Editable = table.Column<bool>(nullable: false),
                    Required = table.Column<bool>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    LastEdit = table.Column<DateTimeOffset>(nullable: false),
                    LastEditUser = table.Column<string>(maxLength: 20, nullable: false)
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
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CheckListId = table.Column<int>(nullable: true),
                    CheckListItemId = table.Column<int>(nullable: true),
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    LastEdit = table.Column<DateTimeOffset>(nullable: false),
                    LastEditUser = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckListParams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckListParams_CheckLists_CheckListId",
                        column: x => x.CheckListId,
                        principalSchema: "Inspections",
                        principalTable: "CheckLists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CheckListParams_CheckListItems_CheckListItemId",
                        column: x => x.CheckListItemId,
                        principalSchema: "Inspections",
                        principalTable: "CheckListItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckListItems_CheckListId",
                schema: "Inspections",
                table: "CheckListItems",
                column: "CheckListId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckListParams_CheckListId",
                schema: "Inspections",
                table: "CheckListParams",
                column: "CheckListId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckListParams_CheckListItemId",
                schema: "Inspections",
                table: "CheckListParams",
                column: "CheckListItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckLists_ReportConfigurationId",
                schema: "Inspections",
                table: "CheckLists",
                column: "ReportConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckLists_ReportId",
                schema: "Inspections",
                table: "CheckLists",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_ReportId",
                schema: "Inspections",
                table: "Notes",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_ReportId",
                schema: "Inspections",
                table: "Photos",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Signatures_ReportConfigurationId",
                schema: "Inspections",
                table: "Signatures",
                column: "ReportConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_Signatures_ReportId",
                schema: "Inspections",
                table: "Signatures",
                column: "ReportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Addresses",
                schema: "Inspections");

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

            migrationBuilder.DropTable(
                name: "Reports",
                schema: "Inspections");
        }
    }
}
