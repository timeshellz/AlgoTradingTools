using System;

namespace AlgoTrading.Stocks.Persistence.Database
{
    public class StockInfoDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string FIGI { get; set; }
        public string Currency { get; set; }
        public string Sector { get; set; }
        public string Country { get; set; }
        public TimeSpan Interval { get; set; }
    }
}
