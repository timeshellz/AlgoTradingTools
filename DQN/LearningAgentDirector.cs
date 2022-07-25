using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using AlgoTrading.Neural;
using AlgoTrading.Broker;
using AlgoTrading.Statistics;
using AlgoTrading.Agent.Learning;
using System.Threading.Tasks;

namespace AlgoTrading.DQN
{
    public class LearningAgentDirector : IStatisticsProvider<LearningDirectorStatistics>
    {       
        private LearningDirectorStatistics statistics = new LearningDirectorStatistics();

        public LearningAgent Agent { get; private set; }
        public IBroker AgentBroker { get; private set; }
        public int EpochSize { get; }

        public LearningAgentDirector(LearningAgent agent)
        {
            Agent = agent;
            AgentBroker = Agent.Broker;

            //TODO: Epoch size should depend on all loaded stocks in the broker, not just single stock data
            EpochSize = AgentBroker.SelectedStockData.Bars.Count;
        }        

        public async Task DirectLearningEpoch()
        {
            await DirectEpoch(true);

            statistics.UpdateCurrentQStatistics(0, 0, Agent.GetEpsilon(), 0, 0, 0);
            statistics.RecordLearningEpoch();

            Agent.DecayEpsilon();
        }

        public async Task DirectSkilledEpoch()
        {
            //TODO: disallow interaction memory collection if becomes necessary

            double currentEpsilon = Agent.GetEpsilon();
            Agent.SetEpsilon(0);

            await DirectEpoch(false);
            statistics.UpdateCurrentQStatistics(0, 0, Agent.GetEpsilon(), 0, 0, 0);
            statistics.RecordSkilledEpoch();

            Agent.SetEpsilon(currentEpsilon);
        }

        private async Task DirectEpoch(bool allowTraining)
        {
            int epochInteractionCount = 0;

            while (epochInteractionCount < EpochSize)
            {
                int iterationInteractionCount = 0;

                while (iterationInteractionCount < Agent.Configuration.BatchSize + Agent.Configuration.MemorySteps)
                {                    
                    var brokerDataNotOver = await Agent.Interact();
                    iterationInteractionCount++;

                    if (!brokerDataNotOver)
                    {
                        if (AgentBroker is IHistoricalBroker historicalBroker)
                            await historicalBroker.GetBeginningTimestep();
                        else
                            throw new Exception("Broker data over.");
                    }
                }

                epochInteractionCount += iterationInteractionCount;

                if (allowTraining && Agent.MemoryBuffer.Count > EpochSize)
                    Agent.Train();

                var iterationStatistics = Agent.GetStatistics();              
                statistics.TotalMemories += iterationStatistics.MemoriesCollected;
                statistics.UpdateCurrentQStatistics(iterationStatistics.TotalReward, iterationStatistics.TotalLoss, 0, 
                    iterationStatistics.QEstimations, 1, iterationStatistics.MemoriesCollected);
                Agent.ResetStatistics();
            }

            var brokerStatistics = AgentBroker.GetStatistics();
            statistics.UpdateCurrentTradeStatistics(brokerStatistics);

            AgentBroker.ResetStatistics();
            Agent.UpdateTargetNetwork();
        }

        public void SaveNetwork()
        {
            //if (AgentSaveManager == null)
            //    throw new NullReferenceException();

            //NeuralSaveManager.SaveNetwork(NeuralNetwork);
            //AgentSaveManager.SaveAgent(Agent);
        }

        public LearningDirectorStatistics GetStatistics()
        {
            return statistics;
        }

        public void ResetStatistics()
        {
            statistics = new LearningDirectorStatistics();
        }
    }
}
