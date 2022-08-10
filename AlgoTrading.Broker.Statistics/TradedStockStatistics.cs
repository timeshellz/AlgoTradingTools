using AlgoTrading.Statistics;
using AlgoTrading.Stocks;
using System;
using System.Collections.Generic;

namespace AlgoTrading.Broker.Statistics
{
    public class TradedStockStatistics : IStatistics
    {
        public string StockName { get; set; }
        public DataInterval Interval { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal StartPrice { get; set; }
        public decimal EndPrice { get; set; }
        public List<MarketPositionStatistics> PositionStatistics { get; set; } = new List<MarketPositionStatistics>();
        public TimeSpan TradeDuration { get; set; } = new TimeSpan(0);
        public decimal Profit { get; set; } = 0;
    }
}
