using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace templater.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    CreateTimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemplateApps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApiRef = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateApps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemplateFormats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContentType = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateFormats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TemplateAppId = table.Column<int>(type: "INTEGER", maxLength: 16, nullable: false),
                    ApiRef = table.Column<string>(type: "TEXT", maxLength: 16, nullable: false),
                    DataSize = table.Column<long>(type: "INTEGER", nullable: false),
                    TemplateFormatId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateTimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastUsedDateTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Templates_TemplateApps_TemplateAppId",
                        column: x => x.TemplateAppId,
                        principalTable: "TemplateApps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Templates_TemplateFormats_TemplateFormatId",
                        column: x => x.TemplateFormatId,
                        principalTable: "TemplateFormats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplateData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateData_Templates_Id",
                        column: x => x.Id,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TemplateFormats",
                columns: new[] { "Id", "ContentType" },
                values: new object[] { 1, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });

            migrationBuilder.InsertData(
                table: "TemplateFormats",
                columns: new[] { "Id", "ContentType" },
                values: new object[] { 2, "application/vnd.openxmlformats-officedocument.wordprocessingml.document" });

            migrationBuilder.CreateIndex(
                name: "IX_Templates_TemplateAppId_ApiRef",
                table: "Templates",
                columns: new[] { "TemplateAppId", "ApiRef" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Templates_TemplateFormatId",
                table: "Templates",
                column: "TemplateFormatId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "TemplateData");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "TemplateApps");

            migrationBuilder.DropTable(
                name: "TemplateFormats");
        }
    }
}
