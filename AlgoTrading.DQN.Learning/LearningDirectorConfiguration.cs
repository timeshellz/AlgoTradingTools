using AlgoTrading.Agent.Learning;
using AlgoTrading.Broker;
using AlgoTrading.Neural;

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
