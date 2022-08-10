using AlgoTrading.Broker;
using AlgoTrading.Neural;
using System.Threading.Tasks;

namespace AlgoTrading.Agent
{
    public interface IAgent
    {
        IBroker Broker { get; }
        NeuralNetwork OnlineNetwork { get; }
        Task<bool> Interact();
    }
}
