using Microsoft.EntityFrameworkCore.Migrations;

namespace BearStock.Dashboard.Migrations
{
    public partial class AddIsDefaultDashboard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "Dashboards",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "Dashboards");
        }
    }
}
