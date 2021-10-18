using Microsoft.EntityFrameworkCore.Migrations;

namespace BearStock.Dashboard.Migrations
{
    public partial class AddedDashboardNameField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Dashboards",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Dashboards");
        }
    }
}
