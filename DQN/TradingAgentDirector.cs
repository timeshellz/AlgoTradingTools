using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using AlgoTrading.Agent;
using AlgoTrading.Broker;

namespace AlgoTrading.DQN
{
    public abstract class TradingAgentDirector
    {
        public TradingAgent Agent { get; private set; }
        public IBroker AgentBroker { get; private set; }

        public TradingAgentDirector(TradingAgent agent)
        {
            Agent = agent;
            AgentBroker = Agent.Broker;
        }

        protected virtual async Task<int> DirectIteration(int maxInteractions)
        {
            int iterationInteractionCount = 0;

            while (iterationInteractionCount < maxInteractions)
            {
                var brokerDataNotOver = await Agent.Interact();
                iterationInteractionCount++;

                if (!brokerDataNotOver)
                {
                    if (AgentBroker is IHistoricalBroker historicalBroker)
                        await historicalBroker.Start();
                    else
                        throw new Exception("Broker data over.");
                }
            }

            return iterationInteractionCount;
        }
    }
}
