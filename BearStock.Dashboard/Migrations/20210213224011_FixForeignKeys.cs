using Microsoft.EntityFrameworkCore.Migrations;

namespace BearStock.Dashboard.Migrations
{
    public partial class FixForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Charts_Dashboards_DashboardId",
                table: "Charts");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Dashboards",
                newName: "Username");

            migrationBuilder.AddForeignKey(
                name: "FK_Charts_Dashboards_DashboardId",
                table: "Charts",
                column: "DashboardId",
                principalTable: "Dashboards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Charts_Dashboards_DashboardId",
                table: "Charts");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Dashboards",
                newName: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Charts_Dashboards_DashboardId",
                table: "Charts",
                column: "DashboardId",
                principalTable: "Dashboards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
