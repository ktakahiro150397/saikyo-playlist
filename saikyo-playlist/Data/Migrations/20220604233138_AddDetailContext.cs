using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace saikyo_playlist.Data.Migrations
{
    public partial class AddDetailContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayListDetailsEntity_PlayListHeaders_PlayListHeadersEntityId",
                table: "PlayListDetailsEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayListDetailsEntity",
                table: "PlayListDetailsEntity");

            migrationBuilder.RenameTable(
                name: "PlayListDetailsEntity",
                newName: "PlayListDetails");

            migrationBuilder.RenameColumn(
                name: "itemId",
                table: "PlayListDetails",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayListDetailsEntity_PlayListHeadersEntityId",
                table: "PlayListDetails",
                newName: "IX_PlayListDetails_PlayListHeadersEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayListDetails",
                table: "PlayListDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayListDetails_PlayListHeaders_PlayListHeadersEntityId",
                table: "PlayListDetails",
                column: "PlayListHeadersEntityId",
                principalTable: "PlayListHeaders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayListDetails_PlayListHeaders_PlayListHeadersEntityId",
                table: "PlayListDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayListDetails",
                table: "PlayListDetails");

            migrationBuilder.RenameTable(
                name: "PlayListDetails",
                newName: "PlayListDetailsEntity");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "PlayListDetailsEntity",
                newName: "itemId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayListDetails_PlayListHeadersEntityId",
                table: "PlayListDetailsEntity",
                newName: "IX_PlayListDetailsEntity_PlayListHeadersEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayListDetailsEntity",
                table: "PlayListDetailsEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayListDetailsEntity_PlayListHeaders_PlayListHeadersEntityId",
                table: "PlayListDetailsEntity",
                column: "PlayListHeadersEntityId",
                principalTable: "PlayListHeaders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
