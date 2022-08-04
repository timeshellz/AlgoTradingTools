using System;
using System.Collections.Generic;
using System.Text;
using AlgoTrading.Agent.Benchmarking;
using AlgoTrading.Neural;
using AlgoTrading.Broker;

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
