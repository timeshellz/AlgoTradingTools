using System;

namespace AlgoTrading.Broker.Statistics.Persistence.Database.DTO
{
    public class MarketPositionStatisticsDTO
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal StartPrice { get; set; }
        public decimal EndPrice { get; set; }
        public int Size { get; set; }
        public decimal Profit { get; set; }
    }
}
