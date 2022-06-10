using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VikopRu.Migrations
{
    public partial class findings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FindingsComments",
                table: "FindingsComments");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FindingsComments");

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Findings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Findings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Findings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Findings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FindingAction",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FindingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FindingAction");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Findings");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Findings");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "Findings");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Findings");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "FindingsComments",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FindingsComments",
                table: "FindingsComments",
                column: "Id");
        }
    }
}
