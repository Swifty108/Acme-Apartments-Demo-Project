using Microsoft.EntityFrameworkCore.Migrations;

namespace Peach_Grove_Apartments_Demo_Project.Data.Migrations
{
    public partial class ChangeFromLastFourOfSSNColumntoSSN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastFourOfSSN",
                table: "Applications");

            migrationBuilder.AddColumn<string>(
                name: "SSN",
                table: "Applications",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SSN",
                table: "Applications");

            migrationBuilder.AddColumn<string>(
                name: "LastFourOfSSN",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
