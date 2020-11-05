using Microsoft.EntityFrameworkCore.Migrations;

namespace Peach_Grove_Apartments_Demo_Project.Data.Migrations
{
    public partial class addiscanceledcolumnapplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isCanceled",
                table: "Applications",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isCanceled",
                table: "Applications");
        }
    }
}
