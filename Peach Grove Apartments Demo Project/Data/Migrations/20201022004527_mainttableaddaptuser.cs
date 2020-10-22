using Microsoft.EntityFrameworkCore.Migrations;

namespace Peach_Grove_Apartments_Demo_Project.data.migrations
{
    public partial class mainttableaddaptuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AptUserId",
                table: "MaintenanceRequests",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_AptUserId",
                table: "MaintenanceRequests",
                column: "AptUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRequests_AspNetUsers_AptUserId",
                table: "MaintenanceRequests",
                column: "AptUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRequests_AspNetUsers_AptUserId",
                table: "MaintenanceRequests");

            migrationBuilder.DropIndex(
                name: "IX_MaintenanceRequests_AptUserId",
                table: "MaintenanceRequests");

            migrationBuilder.DropColumn(
                name: "AptUserId",
                table: "MaintenanceRequests");
        }
    }
}
