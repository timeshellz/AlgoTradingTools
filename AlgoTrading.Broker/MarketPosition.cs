using System;
using System.Collections.Generic;
using System.Text;

using AlgoTrading.Stocks;

namespace AlgoTrading.Broker
{
    public class MarketPosition
    {
        public int Size { get; set; }
        public decimal Commission { get; set; }

        public MarketPosition(int size, decimal commission)
        {
            Size = size;
            Commission = commission;
        }       

        public virtual decimal GetPositionValue(StockBar currentBar)
        {
            return currentBar.Close * Size;
        }

        public virtual decimal GetCurrentValueChange(StockBar currentBar)
        {
            return 0;
        }

        public OpenPosition Open(StockBar startBar)
        {
            return new OpenPosition(startBar, Size, Commission);
        }
    }
}
