using System.Collections.Generic;
using AlgoTrading.Stocks;

namespace AlgoTrading.Broker
{
    public class BrokerConfiguration
    {
        public List<StockIdentifier> StockIdentifiers { get; set; } = new List<StockIdentifier>();
        public decimal StartCapital { get; set; } = 600;
        public decimal Commission { get; set; } = 0.0003M;

        public BrokerConfiguration() { }
    }
}
