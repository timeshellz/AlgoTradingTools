using System;
using System.Collections.Generic;
using System.Text;
using AlgoTrading.Neural;
using AlgoTrading.SimulatedBroker;
using System.IO;

namespace AlgoTrading.DQN
{
    public class DQNMeta
    {
        public string Name { get; set; }
        public string NetworkName { get; set; }
        public string AgentName { get; set; }
        public AgentSettings AgentSettings { get; set; }
        public NeuralSettings NeuralSettings { get; set; }
        public BrokerSettings BrokerSettings { get; set; }
        public DQNStatistics DQNStatistics { get; set; }

        public DQNMeta(string name, string networkFileName, string agentFileName, 
            AgentSettings agentSettings, NeuralSettings neuralSettings, BrokerSettings brokerSettings)
        {
            Name = name;
            NetworkName =  Path.GetFileNameWithoutExtension(networkFileName);
            AgentName = Path.GetFileNameWithoutExtension(agentFileName);
            AgentSettings = agentSettings;
            NeuralSettings = neuralSettings;
            BrokerSettings = brokerSettings;
            DQNStatistics = new DQNStatistics();
        }
    }
}
