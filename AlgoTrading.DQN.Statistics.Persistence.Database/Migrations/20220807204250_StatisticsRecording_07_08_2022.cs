using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace AlgoTrading.DQN.Statistics.Persistence.Database.Migrations
{
    public partial class StatisticsRecording_07_08_2022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LearningStatistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LearningStartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TotalMemories = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningStatistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TradedStockStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Interval = table.Column<TimeSpan>(type: "interval", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    StartPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    EndPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    TradeDuration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Profit = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradedStockStatistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BrokerSessionStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StocksVisited = table.Column<int>(type: "integer", nullable: false),
                    BestTradedStockId = table.Column<int>(type: "integer", nullable: true),
                    TotalTrades = table.Column<int>(type: "integer", nullable: false),
                    TotalTradeDuration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    TotalTradeProfit = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrokerSessionStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrokerSessionStatistics_TradedStockStatistics_BestTradedSto~",
                        column: x => x.BestTradedStockId,
                        principalTable: "TradedStockStatistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MarketPositionStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    StartPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    EndPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Size = table.Column<int>(type: "integer", nullable: false),
                    Profit = table.Column<decimal>(type: "numeric", nullable: false),
                    TradedStockStatisticsDTOId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketPositionStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarketPositionStatistics_TradedStockStatistics_TradedStockS~",
                        column: x => x.TradedStockStatisticsDTOId,
                        principalTable: "TradedStockStatistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EpochStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EpochOrder = table.Column<int>(type: "integer", nullable: false),
                    TotalIterationReward = table.Column<double>(type: "double precision", nullable: false),
                    TotalLoss = table.Column<double>(type: "double precision", nullable: false),
                    FinalEpsilon = table.Column<double>(type: "double precision", nullable: false),
                    MemoriesCount = table.Column<int>(type: "integer", nullable: false),
                    EstimationsCount = table.Column<int>(type: "integer", nullable: false),
                    IterationsCount = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    BrokerSessionStatisticsId = table.Column<int>(type: "integer", nullable: true),
                    LearningSessionStatisticsDTOId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpochStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EpochStatistics_BrokerSessionStatistics_BrokerSessionStatis~",
                        column: x => x.BrokerSessionStatisticsId,
                        principalTable: "BrokerSessionStatistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EpochStatistics_LearningStatistics_LearningSessionStatistic~",
                        column: x => x.LearningSessionStatisticsDTOId,
                        principalTable: "LearningStatistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrokerSessionStatistics_BestTradedStockId",
                table: "BrokerSessionStatistics",
                column: "BestTradedStockId");

            migrationBuilder.CreateIndex(
                name: "IX_EpochStatistics_BrokerSessionStatisticsId",
                table: "EpochStatistics",
                column: "BrokerSessionStatisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_EpochStatistics_LearningSessionStatisticsDTOId",
                table: "EpochStatistics",
                column: "LearningSessionStatisticsDTOId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketPositionStatistics_TradedStockStatisticsDTOId",
                table: "MarketPositionStatistics",
                column: "TradedStockStatisticsDTOId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EpochStatistics");

            migrationBuilder.DropTable(
                name: "MarketPositionStatistics");

            migrationBuilder.DropTable(
                name: "BrokerSessionStatistics");

            migrationBuilder.DropTable(
                name: "LearningStatistics");

            migrationBuilder.DropTable(
                name: "TradedStockStatistics");
        }
    }
}
