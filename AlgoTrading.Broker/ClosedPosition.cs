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

            CommissionedProfit = GetTotalClosingCharge() - GetTotalOpenCharge();
        }

        public TimeSpan GetPositionDuration()
        {
            return EndBar.Date - StartBar.Date;
        }

        public decimal GetTotalClosingCharge()
        {
            if(Size > 0)
                return CloseValue - GetClosingCommissionCharge();
            else
                return CloseValue + GetClosingCommissionCharge();
        }

        public decimal GetClosingCommissionCharge()
        {
            return CloseValue * Commission;
        }
    }
}
