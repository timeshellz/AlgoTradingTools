using Skender.Stock.Indicators;
using System;

namespace AlgoTrading.Stocks
{
    public class StockBar : IQuote
    {
        public DateTime Date { get; set; }
        public decimal Open { get; private set; }
        public decimal Close { get; private set; }
        public decimal High { get; private set; }
        public decimal Low { get; private set; }
        public decimal Volume { get; private set; }

        public StockBar(DateTime time, decimal open, decimal close, decimal high, decimal low, decimal volume)
        {
            Date = time;
            Open = open;
            Close = close;
            High = high;
            Low = low;
            Volume = volume;
        }
    }
}
