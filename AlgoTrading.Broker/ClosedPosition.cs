using System;
using System.Collections.Generic;
using System.Text;
using AlgoTrading.Stocks;

namespace AlgoTrading.Broker
{
    public class ClosedPosition : OpenPosition
    {
        public StockBar EndBar { get; }

        public decimal CloseValue { get; }
        public decimal ValueChange { get; }
        public decimal CommissionedProfit { get; }

        public ClosedPosition(StockBar startBar, StockBar endBar, int size, decimal commission) :
            base(startBar, size, commission)
        {
            EndBar = endBar;
            CloseValue = endBar.Close * Size;
            ValueChange = EndBar.Close * Size - OpeningValue;

            var openCharge = GetTotalOpenCharge();
            var closingCharge = GetTotalClosingCharge();

            CommissionedProfit = closingCharge - openCharge;
        }

        public TimeSpan GetPositionDuration()
        {
            return EndBar.Date - StartBar.Date;
        }

        public decimal GetTotalClosingCharge()
        {
            return GetProjectedClosingCharge(EndBar);
        }

        public decimal GetClosingCommissionCharge()
        {
            return GetProjectedCommisionCharge(EndBar);
        }
    }
}
