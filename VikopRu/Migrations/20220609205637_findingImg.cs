using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VikopRu.Migrations
{
    public partial class findingImg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Findings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Findings");
        }
    }
}
