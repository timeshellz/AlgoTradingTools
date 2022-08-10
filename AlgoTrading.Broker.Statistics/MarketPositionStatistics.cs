using AlgoTrading.Broker.Positions;
using System;

namespace AlgoTrading.Broker.Statistics
{
    public class MarketPositionStatistics
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal StartPrice { get; set; }
        public decimal EndPrice { get; set; }
        public int Size { get; set; }
        public decimal Profit { get; set; }

        public MarketPositionStatistics() { }

        public MarketPositionStatistics(MarketPosition position)
        {
            if (position is OpenPosition openPosition)
            {
                StartDate = openPosition.StartBar.Date;
                StartPrice = openPosition.StartBar.Close;
                Size = openPosition.Size;

                if (openPosition is ClosedPosition closedPosition)
                {
                    EndDate = closedPosition.EndBar.Date;
                    EndPrice = closedPosition.EndBar.Close;
                    Profit = closedPosition.CommissionedProfit;
                }
            }
        }
    }
}
