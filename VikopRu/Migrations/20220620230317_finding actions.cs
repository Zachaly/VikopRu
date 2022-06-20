using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VikopRu.Migrations
{
    public partial class findingactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FindingAction",
                table: "FindingAction");

            migrationBuilder.RenameTable(
                name: "FindingAction",
                newName: "FindingActions");

            migrationBuilder.AddColumn<bool>(
                name: "IsDig",
                table: "FindingActions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FindingActions",
                table: "FindingActions",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FindingActions",
                table: "FindingActions");

            migrationBuilder.DropColumn(
                name: "IsDig",
                table: "FindingActions");

            migrationBuilder.RenameTable(
                name: "FindingActions",
                newName: "FindingAction");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FindingAction",
                table: "FindingAction",
                column: "Id");
        }
    }
}
