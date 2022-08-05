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
            if(Size > 0)
                return OpeningValue + GetOpenCommissionCharge();
            else
                return OpeningValue - GetOpenCommissionCharge();
        }

        public decimal GetOpenCommissionCharge()
        {
            return GetProjectedCommisionCharge(StartBar);
        }

        public ClosedPosition Close(StockBar closeBar)
        {
            return new ClosedPosition(StartBar, closeBar, Size, Commission);
        }

        public decimal GetProjectedClosingCharge(StockBar projectedBar)
        {
            if (Size > 0)
                return projectedBar.Close * Size - GetProjectedCommisionCharge(projectedBar);
            else
                return projectedBar.Close * Size + GetProjectedCommisionCharge(projectedBar);
        }       
    }
}
