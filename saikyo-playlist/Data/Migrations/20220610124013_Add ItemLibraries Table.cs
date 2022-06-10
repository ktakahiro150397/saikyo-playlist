using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace saikyo_playlist.Data.Migrations
{
    public partial class AddItemLibrariesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "PlayListDetails");

            migrationBuilder.DropColumn(
                name: "PlayCount",
                table: "PlayListDetails");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "PlayListDetails");

            migrationBuilder.DropColumn(
                name: "TitleAlias",
                table: "PlayListDetails");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "PlayListDetails");

            migrationBuilder.AddColumn<string>(
                name: "ItemLibrariesEntityId",
                table: "PlayListDetails",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ItemLibraries",
                columns: table => new
                {
                    ItemLibrariesEntityId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "ライブラリアイテムごとに採番されるユニークなID。"),
                    AspNetUserdId = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "このアイテムを所有しているユーザーのID。"),
                    Platform = table.Column<int>(type: "int", nullable: false, comment: "プレイリストのプラットフォーム種別。"),
                    ItemId = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "アイテムを特定するためのプラットフォームごとのID。"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "アイテムの元々の名称。"),
                    TitleAlias = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "ユーザーによって付けられたアイテムの別名。"),
                    PlayCount = table.Column<int>(type: "int", nullable: false, comment: "アイテムが最後まで再生された数。"),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false, comment: "タイムスタンプ。")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemLibraries", x => x.ItemLibrariesEntityId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayListDetails_ItemLibrariesEntityId",
                table: "PlayListDetails",
                column: "ItemLibrariesEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayListDetails_ItemLibraries_ItemLibrariesEntityId",
                table: "PlayListDetails",
                column: "ItemLibrariesEntityId",
                principalTable: "ItemLibraries",
                principalColumn: "ItemLibrariesEntityId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayListDetails_ItemLibraries_ItemLibrariesEntityId",
                table: "PlayListDetails");

            migrationBuilder.DropTable(
                name: "ItemLibraries");

            migrationBuilder.DropIndex(
                name: "IX_PlayListDetails_ItemLibrariesEntityId",
                table: "PlayListDetails");

            migrationBuilder.DropColumn(
                name: "ItemLibrariesEntityId",
                table: "PlayListDetails");

            migrationBuilder.AddColumn<string>(
                name: "ItemId",
                table: "PlayListDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "アイテムを特定するためのプラットフォームごとのID。");

            migrationBuilder.AddColumn<int>(
                name: "PlayCount",
                table: "PlayListDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "アイテムがプレイリスト内で最後まで再生された数。");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "PlayListDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "アイテムの元々の名称。");

            migrationBuilder.AddColumn<string>(
                name: "TitleAlias",
                table: "PlayListDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "ユーザーによって付けられたアイテムの別名。");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "PlayListDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "プレイリストのプラットフォーム種別。");
        }
    }
}
