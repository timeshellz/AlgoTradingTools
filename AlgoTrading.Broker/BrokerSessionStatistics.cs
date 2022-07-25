using System;
using System.Collections.Generic;
using System.Text;
using AlgoTrading.Statistics;
using AlgoTrading.Stocks;

namespace AlgoTrading.Broker
{
    public class BrokerSessionStatistics : IStatistics
    {
        private int totalTrades;
        private TimeSpan totalTradeDuration = TimeSpan.FromSeconds(0);
        private decimal totalTradeProfit;

        public TradedStockStatistics BestTradedStock { get; set; }       
        public TimeSpan AverageTradeDuration { get; private set; }       
        public decimal AverageTradeProfit { get; set; } = 0;
        public int TotalTrades 
        { 
            get => totalTrades; 
            set
            {
                totalTrades = value;

                CalculateAverageDuration();
                CalculateAverageProfit();
            }
        }

        public TimeSpan TotalTradeDuration 
        { 
            get => totalTradeDuration; 
            set
            {
                totalTradeDuration = value;

                CalculateAverageDuration();
            }
        }

        public decimal TotalTradeProfit 
        { 
            get => totalTradeProfit; 
            set
            {
                totalTradeProfit = value;

                CalculateAverageProfit();
            }
        }

        private void CalculateAverageDuration()
        {
            if (TotalTrades > 0)
                AverageTradeDuration = TotalTradeDuration / TotalTrades;
            else
                AverageTradeDuration = new TimeSpan(0);
        }

        private void CalculateAverageProfit()
        {
            if (TotalTrades > 0)
                AverageTradeProfit = TotalTradeProfit / TotalTrades;
            else
                AverageTradeProfit = 0;
        }
    }

    public class TradedStockStatistics : IStatistics
    {
        public StockData StockData { get; set; }
        public List<MarketPosition> Positions { get; set; } = new List<MarketPosition>();
        public TimeSpan TradeDuration { get; set; } = new TimeSpan(0);
        public decimal Profit { get; set; } = 0;
    }
}
