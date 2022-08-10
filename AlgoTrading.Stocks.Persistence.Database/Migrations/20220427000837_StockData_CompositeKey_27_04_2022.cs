using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;

namespace AlgoTrading.Stocks.Persistence.Database.Migrations
{
    public partial class StockData_CompositeKey_27_04_2022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockInfos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    FIGI = table.Column<string>(type: "text", nullable: true),
                    Currency = table.Column<string>(type: "text", nullable: true),
                    Sector = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockInfos", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    FIGI = table.Column<string>(type: "text", nullable: false),
                    Interval = table.Column<TimeSpan>(type: "interval", nullable: false),
                    InfoID = table.Column<int>(type: "integer", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => new { x.FIGI, x.Interval });
                    table.ForeignKey(
                        name: "FK_Stocks_StockInfos_InfoID",
                        column: x => x.InfoID,
                        principalTable: "StockInfos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bars",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Open = table.Column<decimal>(type: "numeric", nullable: false),
                    Close = table.Column<decimal>(type: "numeric", nullable: false),
                    High = table.Column<decimal>(type: "numeric", nullable: false),
                    Low = table.Column<decimal>(type: "numeric", nullable: false),
                    Volume = table.Column<decimal>(type: "numeric", nullable: false),
                    StockDataDTOFIGI = table.Column<string>(type: "text", nullable: true),
                    StockDataDTOInterval = table.Column<TimeSpan>(type: "interval", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bars", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Bars_Stocks_StockDataDTOFIGI_StockDataDTOInterval",
                        columns: x => new { x.StockDataDTOFIGI, x.StockDataDTOInterval },
                        principalTable: "Stocks",
                        principalColumns: new[] { "FIGI", "Interval" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bars_StockDataDTOFIGI_StockDataDTOInterval",
                table: "Bars",
                columns: new[] { "StockDataDTOFIGI", "StockDataDTOInterval" });

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_InfoID",
                table: "Stocks",
                column: "InfoID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bars");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "StockInfos");
        }
    }
}
