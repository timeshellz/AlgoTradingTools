using System.Threading.Tasks;

namespace AlgoTrading.Broker
{
    public interface IHistoricalBroker : IBroker
    {
        Task<MarketState> GetBeginningTimestep();
    }
}
