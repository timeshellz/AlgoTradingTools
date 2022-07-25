using System;
using System.Collections.Generic;
using System.Text;
using AlgoTrading.Stocks;

namespace AlgoTrading.Broker
{
    public class OpenPosition : MarketPosition
    {
        public StockBar StartBar { get;  }

        public decimal OpeningValue { get; }

        public OpenPosition(StockBar startBar, int size, decimal commission) : base(size, commission)
        {
            StartBar = startBar;
            OpeningValue = StartBar.Close * Size;
        }

        public override decimal GetCurrentValueChange(StockBar currentBar)
        {
            return GetPositionValue(currentBar) - OpeningValue;
        }

        public decimal GetTotalOpenCharge()
        {
            return OpeningValue + GetOpenCommissionCharge();
        }

        public decimal GetOpenCommissionCharge()
        {
            return OpeningValue * Commission;
        }

        public ClosedPosition Close(StockBar closeBar)
        {
            return new ClosedPosition(StartBar, closeBar, Size, Commission);
        }
    }
}
