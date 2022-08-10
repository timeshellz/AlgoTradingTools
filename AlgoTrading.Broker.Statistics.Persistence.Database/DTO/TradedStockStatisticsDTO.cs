using System;
using System.Collections.Generic;

namespace AlgoTrading.Broker.Statistics.Persistence.Database.DTO
{
    public class TradedStockStatisticsDTO
    {
        public int Id { get; set; }
        public TimeSpan Interval { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal StartPrice { get; set; }
        public decimal EndPrice { get; set; }
        public List<MarketPositionStatisticsDTO> Positions { get; set; } = new List<MarketPositionStatisticsDTO>();
        public TimeSpan TradeDuration { get; set; } = new TimeSpan(0);
        public decimal Profit { get; set; } = 0;
    }
}
