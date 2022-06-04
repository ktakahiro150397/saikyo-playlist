using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace saikyo_playlist.Data.Migrations
{
    public partial class PlayListTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayListHeaders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "プレイリストごとに採番されるユニークなID。"),
                    AspNetUserdId = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "このプレイリストを作成したユーザーのID。"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "プレイリストの名前。"),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false, comment: "タイムスタンプ。")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayListHeaders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayListDetailsEntity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "プレイリスト詳細ごとに採番されるユニークなID。"),
                    ItemSeq = table.Column<int>(type: "int", nullable: false, comment: "0から始まるプレイリストの連番。"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "プレイリストのプラットフォーム種別。"),
                    itemId = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "アイテムを特定するためのプラットフォームごとのID。"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "アイテムの元々の名称。"),
                    TitleAlias = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "ユーザーによって付けられたアイテムの別名。"),
                    PlayCount = table.Column<int>(type: "int", nullable: false, comment: "アイテムがプレイリスト内で最後まで再生された数。"),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false, comment: "タイムスタンプ。"),
                    PlayListHeadersEntityId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayListDetailsEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayListDetailsEntity_PlayListHeaders_PlayListHeadersEntityId",
                        column: x => x.PlayListHeadersEntityId,
                        principalTable: "PlayListHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayListDetailsEntity_PlayListHeadersEntityId",
                table: "PlayListDetailsEntity",
                column: "PlayListHeadersEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayListDetailsEntity");

            migrationBuilder.DropTable(
                name: "PlayListHeaders");
        }
    }
}
