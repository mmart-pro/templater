using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace templater.Migrations
{
    public partial class TemplateApps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Templates_TemplateApp_TemplateAppId",
                table: "Templates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TemplateApp",
                table: "TemplateApp");

            migrationBuilder.RenameTable(
                name: "TemplateApp",
                newName: "TemplateApps");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TemplateApps",
                table: "TemplateApps",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_TemplateApps_TemplateAppId",
                table: "Templates",
                column: "TemplateAppId",
                principalTable: "TemplateApps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Templates_TemplateApps_TemplateAppId",
                table: "Templates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TemplateApps",
                table: "TemplateApps");

            migrationBuilder.RenameTable(
                name: "TemplateApps",
                newName: "TemplateApp");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TemplateApp",
                table: "TemplateApp",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_TemplateApp_TemplateAppId",
                table: "Templates",
                column: "TemplateAppId",
                principalTable: "TemplateApp",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
