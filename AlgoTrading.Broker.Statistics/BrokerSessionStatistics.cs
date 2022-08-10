using AlgoTrading.Statistics;
using System;

namespace AlgoTrading.Broker.Statistics
{
    public class BrokerSessionStatistics : IStatistics
    {
        public int TotalStocksVisited { get; set; }
        public TradedStockStatistics BestTradedStock { get; set; }
        public TimeSpan AverageTradeDuration
        {
            get
            {
                if (TotalTrades == 0)
                    return TimeSpan.Zero;
                else
                    return TotalTradeDuration / TotalTrades;
            }
        }

        public decimal AverageTradeProfit
        {
            get
            {
                if (TotalTrades == 0)
                    return 0;
                else
                    return TotalTradeProfit / TotalTrades;
            }
        }
        public int TotalTrades { get; set; }

        public TimeSpan TotalTradeDuration { get; set; }

        public decimal TotalTradeProfit { get; set; }
    }
}
