using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace saikyo_playlist.Data.Migrations
{
    public partial class AddLibraryItemAddDateColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ItemAddDate",
                table: "ItemLibraries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "アイテムがライブラリに追加された日付");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemAddDate",
                table: "ItemLibraries");
        }
    }
}
