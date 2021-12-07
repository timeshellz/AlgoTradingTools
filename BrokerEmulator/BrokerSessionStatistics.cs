using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoTrading.SimulatedBroker
{
    public struct BrokerSessionStatistics
    {
        public double TotalTradeDuration { get; set; }
        public double TotalCommissionedTradeProfit { get; set; }
        public int TotalTrades { get; set; }

        public BrokerSessionStatistics(double totalTradeDuration, double totalTradeProfit, int totalTrades)
        {
            TotalTradeDuration = totalTradeDuration;
            TotalCommissionedTradeProfit = totalTradeProfit;
            TotalTrades = totalTrades;
        }
    }
}
