using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlgoTrading.Stocks.Persistence.Database.Migrations
{
    public partial class StockIntervalMigration_07_08_2022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bars_Stocks_StockDataDTOFIGI_StockDataDTOInterval",
                table: "Bars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stocks",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_Bars_StockDataDTOFIGI_StockDataDTOInterval",
                table: "Bars");

            migrationBuilder.DropColumn(
                name: "Interval",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "StockDataDTOInterval",
                table: "Bars");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Stocks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Interval",
                table: "StockInfos",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "StockDataDTOId",
                table: "Bars",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stocks",
                table: "Stocks",
                columns: new[] { "FIGI", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Bars_StockDataDTOFIGI_StockDataDTOId",
                table: "Bars",
                columns: new[] { "StockDataDTOFIGI", "StockDataDTOId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Bars_Stocks_StockDataDTOFIGI_StockDataDTOId",
                table: "Bars",
                columns: new[] { "StockDataDTOFIGI", "StockDataDTOId" },
                principalTable: "Stocks",
                principalColumns: new[] { "FIGI", "Id" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bars_Stocks_StockDataDTOFIGI_StockDataDTOId",
                table: "Bars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stocks",
                table: "Stocks");

            migrationBuilder.DropIndex(
                name: "IX_Bars_StockDataDTOFIGI_StockDataDTOId",
                table: "Bars");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "Interval",
                table: "StockInfos");

            migrationBuilder.DropColumn(
                name: "StockDataDTOId",
                table: "Bars");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Interval",
                table: "Stocks",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StockDataDTOInterval",
                table: "Bars",
                type: "interval",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stocks",
                table: "Stocks",
                columns: new[] { "FIGI", "Interval" });

            migrationBuilder.CreateIndex(
                name: "IX_Bars_StockDataDTOFIGI_StockDataDTOInterval",
                table: "Bars",
                columns: new[] { "StockDataDTOFIGI", "StockDataDTOInterval" });

            migrationBuilder.AddForeignKey(
                name: "FK_Bars_Stocks_StockDataDTOFIGI_StockDataDTOInterval",
                table: "Bars",
                columns: new[] { "StockDataDTOFIGI", "StockDataDTOInterval" },
                principalTable: "Stocks",
                principalColumns: new[] { "FIGI", "Interval" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
