using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Peach_Grove_Apartments_Demo_Project.data.migrations
{
    public partial class daterequestfieldstodatetype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateRequested",
                table: "MaintenanceRequests",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateRequested",
                table: "MaintenanceRequests",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}
