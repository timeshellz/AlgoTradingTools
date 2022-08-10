using AlgoTrading.Agent.Learning;
using AlgoTrading.DQN.Statistics;
using AlgoTrading.Statistics;
using System.Threading.Tasks;
using AlgoTrading.DQN.Statistics.Persistence;

namespace AlgoTrading.DQN.Learning
{
    public class LearningAgentDirector : TradingAgentDirector, IStatisticsProvider<LearningDirectorStatistics>
    {
        private LearningDirectorStatistics statistics = new LearningDirectorStatistics();

        private ILearningStatisticsPersistenceManager persistenceManager;

        new public LearningAgent Agent { get; private set; }
        public int EpochSize { get; }

        public LearningAgentDirector(LearningAgent agent, ILearningStatisticsPersistenceManager persistenceManager) : base(agent)
        {
            Agent = agent;
            //TODO: Epoch size should depend on all loaded stocks in the broker, not just single stock data
            EpochSize = AgentBroker.SelectedStockData.Bars.Count;
            this.persistenceManager = persistenceManager;
        }

        public async Task DirectLearningEpoch()
        {
            await DirectEpoch(true);

            statistics.RecordFinalEpsilon(Agent.GetEpsilon());
            statistics.RecordLearningEpoch();

            Agent.DecayEpsilon();
        }

        public async Task DirectSkilledEpoch()
        {
            double currentEpsilon = Agent.GetEpsilon();
            Agent.SetEpsilon(0);

            await DirectEpoch(false);
            statistics.RecordFinalEpsilon(Agent.GetEpsilon());
            statistics.RecordSkilledEpoch();

            Agent.SetEpsilon(currentEpsilon);
        }

        private async Task DirectEpoch(bool allowTraining)
        {
            int epochInteractionCount = 0;

            var brokerStatistics = AgentBroker.GetStatistics();
            while (brokerStatistics.TotalStocksVisited <= AgentBroker.Configuration.StockIdentifiers.Count)
            {
                epochInteractionCount += await DirectIteration(Agent.Configuration.BatchSize + Agent.Configuration.MemorySteps, allowTraining);

                if (allowTraining && (Agent.MemoryBuffer.Count > EpochSize || Agent.MemoryBuffer.Size == Agent.MemoryBuffer.Count))
                    Agent.Train();

                var iterationStatistics = Agent.GetStatistics();
                statistics.RecordIteration(iterationStatistics);
                Agent.ResetStatistics();
            }

            statistics.UpdateCurrentTradeStatistics(brokerStatistics);

            AgentBroker.ResetStatistics();

            await persistenceManager.SaveStatistics(statistics);
            await AgentBroker.Start();
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
