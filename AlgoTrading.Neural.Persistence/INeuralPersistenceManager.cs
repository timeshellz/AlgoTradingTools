using System;
using System.Threading.Tasks;
using AlgoTrading.Neural;

namespace AlgoTrading.Neural.Persistence
{
    public interface INeuralPersistenceManager
    {
        Task<NeuralNetwork> LoadNeuralNetwork(string networkName);
        Task SaveNeuralNetwork(NeuralNetwork network);
    }
}
