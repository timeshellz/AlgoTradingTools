using System;

namespace AlgoTrading.Broker.Statistics.Persistence.Database.DTO
{
    public class BrokerSessionStatisticsDTO
    {
        public int Id { get; set; }
        public int StocksVisited { get; set; }
        public TradedStockStatisticsDTO BestTradedStock { get; set; }
        public int TotalTrades { get; set; }
        public TimeSpan TotalTradeDuration { get; set; }
        public decimal TotalTradeProfit { get; set; }
    }
}
