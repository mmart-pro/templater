using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace templater.Migrations
{
    public partial class TemplateDatas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemplateData_Templates_Id",
                table: "TemplateData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TemplateData",
                table: "TemplateData");

            migrationBuilder.RenameTable(
                name: "TemplateData",
                newName: "TemplateDatas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TemplateDatas",
                table: "TemplateDatas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateDatas_Templates_Id",
                table: "TemplateDatas",
                column: "Id",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemplateDatas_Templates_Id",
                table: "TemplateDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TemplateDatas",
                table: "TemplateDatas");

            migrationBuilder.RenameTable(
                name: "TemplateDatas",
                newName: "TemplateData");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TemplateData",
                table: "TemplateData",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateData_Templates_Id",
                table: "TemplateData",
                column: "Id",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
