using Microsoft.EntityFrameworkCore.Migrations;

namespace BearStock.Dashboard.Migrations
{
    public partial class SetUserIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Dashboards",
                newName: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Dashboards",
                newName: "Username");
        }
    }
}
