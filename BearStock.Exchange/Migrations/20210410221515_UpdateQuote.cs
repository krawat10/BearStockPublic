using Microsoft.EntityFrameworkCore.Migrations;

namespace BearStock.Exchange.Migrations
{
    public partial class UpdateQuote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quotas_FromCurrency_ToCurrency",
                table: "Quotas");

            migrationBuilder.DropColumn(
                name: "FromCurrency",
                table: "Quotas");

            migrationBuilder.DropColumn(
                name: "ToCurrency",
                table: "Quotas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FromCurrency",
                table: "Quotas",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToCurrency",
                table: "Quotas",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quotas_FromCurrency_ToCurrency",
                table: "Quotas",
                columns: new[] { "FromCurrency", "ToCurrency" });
        }
    }
}
