using System;
using System.Collections.Generic;
using System.Text;
using AlgoTrading.Agent;
using AlgoTrading.Neural;
using AlgoTrading.Broker;

namespace AlgoTrading.DQN
{
    public class AgentDirectorConfiguration
    {
        public AgentConfiguration AgentConfiguration { get; set; }
        public NeuralConfiguration NeuralConfiguration { get; set; }
        public BrokerConfiguration BrokerConfiguration { get; set; }

        public AgentDirectorConfiguration(AgentConfiguration agentSettings, NeuralConfiguration neuralSettings, BrokerConfiguration brokerSettings)
        {
            AgentConfiguration = agentSettings;
            NeuralConfiguration = neuralSettings;
            BrokerConfiguration = brokerSettings;
            ValidateSettings();
        }

        private bool ValidateSettings()
        {
            if (BrokerConfiguration == null)
                throw new ArgumentNullException("Broker settings not set.");

            if (NeuralConfiguration == null)
                throw new ArgumentNullException("Neural settings not set.");

            if (AgentConfiguration == null)
                throw new ArgumentNullException("Agent settings not set.");

            return true;
        }
    }
}
