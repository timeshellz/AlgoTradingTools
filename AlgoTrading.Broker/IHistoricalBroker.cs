using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTrading.Broker
{
    public interface IHistoricalBroker : IBroker
    {
        Task<MarketState> GetBeginningTimestep();
    }
}
