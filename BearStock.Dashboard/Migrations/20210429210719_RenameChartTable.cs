using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BearStock.Dashboard.Migrations
{
    public partial class RenameChartTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Charts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dashboards",
                table: "Dashboards");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Dashboards");

            migrationBuilder.AddColumn<Guid>(
                name: "Uuid",
                table: "Dashboards",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dashboards",
                table: "Dashboards",
                column: "Uuid");

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Ticket = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    DashboardUuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_Stocks_Dashboards_DashboardUuid",
                        column: x => x.DashboardUuid,
                        principalTable: "Dashboards",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockPositions",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    StockUuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PricePerShare = table.Column<double>(type: "float", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SharesAmount = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockPositions", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_StockPositions_Stocks_StockUuid",
                        column: x => x.StockUuid,
                        principalTable: "Stocks",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockPositions_StockUuid",
                table: "StockPositions",
                column: "StockUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_DashboardUuid",
                table: "Stocks",
                column: "DashboardUuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockPositions");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dashboards",
                table: "Dashboards");

            migrationBuilder.DropColumn(
                name: "Uuid",
                table: "Dashboards");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Dashboards",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dashboards",
                table: "Dashboards",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Charts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DashboardId = table.Column<int>(type: "int", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Ticket = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Charts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Charts_Dashboards_DashboardId",
                        column: x => x.DashboardId,
                        principalTable: "Dashboards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Charts_DashboardId",
                table: "Charts",
                column: "DashboardId");
        }
    }
}
