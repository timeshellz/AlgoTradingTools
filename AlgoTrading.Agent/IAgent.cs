using System;
using System.Threading.Tasks;
using AlgoTrading.Neural;
using AlgoTrading.Broker;

namespace AlgoTrading.Agent
{
    public interface IAgent
    {
        IBroker Broker { get; }
        NeuralNetwork OnlineNetwork { get; }
        Task<bool> Interact();     
    }
}
