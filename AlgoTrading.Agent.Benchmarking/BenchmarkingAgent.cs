using AlgoTrading.Broker;
using AlgoTrading.Neural;

namespace AlgoTrading.Agent.Benchmarking
{
    public class BenchmarkingAgent : TradingAgent
    {
        public BenchmarkingAgentConfiguration Configuration { get; private set; }

        public BenchmarkingAgent(BenchmarkingAgentConfiguration configuration, NeuralNetwork network, IBroker broker) : base(network, broker)
        {
            Configuration = configuration;
        }
    }
}
