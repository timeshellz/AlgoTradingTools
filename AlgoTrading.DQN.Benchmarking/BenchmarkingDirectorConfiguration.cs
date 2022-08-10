using AlgoTrading.Agent.Benchmarking;
using AlgoTrading.Broker;
using AlgoTrading.Neural;

namespace AlgoTrading.DQN.Benchmarking
{
    class BenchmarkingDirectorConfiguration : AgentDirectorConfiguration
    {
        public new BenchmarkingAgentConfiguration AgentConfiguration { get; set; }

        public BenchmarkingDirectorConfiguration(BenchmarkingAgentConfiguration agentSettings, NeuralConfiguration neuralSettings, BrokerConfiguration brokerSettings)
            : base(agentSettings, neuralSettings, brokerSettings)
        {
            AgentConfiguration = agentSettings;
        }
    }
}
