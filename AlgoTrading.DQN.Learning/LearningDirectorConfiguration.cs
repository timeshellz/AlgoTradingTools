using System;
using System.Collections.Generic;
using System.Text;
using AlgoTrading.Neural;
using AlgoTrading.Broker;
using AlgoTrading.Agent;
using AlgoTrading.Agent.Learning;
using System.IO;

namespace AlgoTrading.DQN.Learning
{
    public class LearningDirectorConfiguration : AgentDirectorConfiguration
    {
        public new LearningAgentConfiguration AgentConfiguration { get; set; }

        public LearningDirectorConfiguration(LearningAgentConfiguration agentSettings, NeuralConfiguration neuralSettings, BrokerConfiguration brokerSettings) 
            : base(agentSettings, neuralSettings, brokerSettings)
        {
            AgentConfiguration = agentSettings;
        }
    }
}
