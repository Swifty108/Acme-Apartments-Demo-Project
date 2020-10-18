using Microsoft.EntityFrameworkCore.Migrations;

namespace Peach_Grove_Apartments_Demo_Project.Data.Migrations
{
    public partial class AddRoomAndPriceColumnsInAppTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Price",
                table: "Applications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Room",
                table: "Applications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Room",
                table: "Applications");
        }
    }
}
