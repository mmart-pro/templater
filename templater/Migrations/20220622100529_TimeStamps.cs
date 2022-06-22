using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace templater.Migrations
{
    public partial class TimeStamps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUsedDateTime",
                table: "Templates",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTimeStamp",
                table: "Templates",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeStamp",
                table: "Templates",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateTimeStamp",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "Templates");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUsedDateTime",
                table: "Templates",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
