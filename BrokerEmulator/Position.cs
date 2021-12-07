using System;
using System.Collections.Generic;
using System.Text;

using AlgoTrading.Stocks;

namespace AlgoTrading.SimulatedBroker
{
    public class Position
    {
        public string StockName { get; private set; }
        public bool IsComplete { get; private set; }
        public int Size { get; private set; }
        public int CurrentSize { get; private set; }
        public decimal TotalStartPrice { get; private set; }
        public decimal TotalCommissionedStartPrice { get; private set; }
        public decimal StartPrice { get; private set; }
        public decimal TotalEndPrice { get; private set; }
        public decimal TotalCommissionedEndPrice { get; private set; }
        public decimal EndPrice { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public decimal Profit { get; private set; }
        public decimal CommissionedProfit { get; private set; }
        public decimal ProfitPercentage { get; private set; }

        public decimal Enter(string stockName, int size, StockBar dataBar, decimal commission)
        {
            if (IsComplete)
                throw new ArgumentException();

            StockName = stockName;
            IsComplete = false;
            Size = size;
            CurrentSize = Size;
            StartDate = dataBar.Date;
            StartPrice = dataBar.Close;
            TotalStartPrice = Size * StartPrice;
            TotalCommissionedStartPrice = (StartPrice + dataBar.Close * commission) * Size;

            return TotalCommissionedStartPrice;
        }

        public decimal GetEquityChange(StockBar currentBar)
        {
            return currentBar.Close * CurrentSize - TotalStartPrice;
        }

        public decimal GetCommissionedEquityChange(StockBar currentBar)
        {
            return currentBar.Close * CurrentSize - TotalCommissionedStartPrice;
        }

        public decimal GetPositionEquity(StockBar currentBar)
        {
            return currentBar.Close * Size;
        }

        public decimal ExitAll(StockBar exitBar, decimal commission)
        {
            if (IsComplete)
                throw new ArgumentException();

            IsComplete = true;
            EndDate = exitBar.Date;
            EndPrice = exitBar.Close;
            TotalEndPrice = EndPrice * CurrentSize;
            CurrentSize = 0;

            TotalCommissionedEndPrice = (EndPrice - exitBar.Close * commission) * Size;

            Profit = TotalEndPrice - TotalStartPrice;
            CommissionedProfit = TotalCommissionedEndPrice - TotalCommissionedStartPrice;
            ProfitPercentage = (EndPrice / StartPrice) - 1;

            return TotalCommissionedEndPrice;
        }
    }
}
