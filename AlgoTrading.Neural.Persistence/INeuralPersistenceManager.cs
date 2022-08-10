using System.Threading.Tasks;

namespace AlgoTrading.Neural.Persistence
{
    public interface INeuralPersistenceManager
    {
        Task<NeuralNetwork> LoadNeuralNetwork(string networkName);
        Task SaveNeuralNetwork(NeuralNetwork network);
    }
}
