using Microsoft.EntityFrameworkCore.Migrations;

namespace Peach_Grove_Apartments_Demo_Project.data.migrations
{
    public partial class isapprovedtomaintenance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isApproved",
                table: "MaintenanceRequests",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isApproved",
                table: "MaintenanceRequests");
        }
    }
}
