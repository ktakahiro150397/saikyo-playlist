using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace saikyo_playlist.Data.Migrations
{
    public partial class ChangeIdColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PlayListHeaders",
                newName: "PlayListHeadersEntityId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PlayListDetails",
                newName: "PlayListDetailsEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlayListHeadersEntityId",
                table: "PlayListHeaders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PlayListDetailsEntityId",
                table: "PlayListDetails",
                newName: "Id");
        }
    }
}
