using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoTrading.Stocks
{
    public class IntervalStockIdentifier
    {
        public DataInterval Interval { get; set; }
        public StockIdentifier Identifier { get; set; }

        public IntervalStockIdentifier(DataInterval interval, StockIdentifier identifier)
        {
            Interval = interval;
            Identifier = identifier;
        }
    }
}
