using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VikopRu.Migrations
{
    public partial class commentid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "FindingsComments",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "FindingAction",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FindingsComments",
                table: "FindingsComments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FindingAction",
                table: "FindingAction",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FindingsComments",
                table: "FindingsComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FindingAction",
                table: "FindingAction");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FindingsComments");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FindingAction");
        }
    }
}
