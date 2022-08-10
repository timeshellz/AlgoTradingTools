using System;

namespace AlgoTrading.Stocks.Persistence.Database
{
    public class StockBarDTO
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Volume { get; set; }
    }
}
