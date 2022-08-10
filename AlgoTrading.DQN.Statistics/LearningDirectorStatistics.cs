using AlgoTrading.Agent.Statistics;
using AlgoTrading.Broker.Statistics;
using AlgoTrading.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgoTrading.DQN.Statistics
{
    public class LearningDirectorStatistics : IStatistics
    {
        private double maxIterationReward = double.MinValue;
        private double maxSkilledReward = double.MinValue;

        public Guid UUID { get; set; }
        public DateTime LearningStartDate { get; set; }
        public int TotalMemories { get; set; }
        public List<EpochStatistics> LearningEpochs { get; set; } = new List<EpochStatistics>();
        public List<EpochStatistics> SkilledEpochs { get; set; } = new List<EpochStatistics>();
        public EpochStatistics BestSkilledEpoch { get; set; }
        public EpochStatistics CurrentEpoch { get; set; }

        public LearningDirectorStatistics()
        {
            UUID = Guid.NewGuid();

            CreateNewEpoch();
        }

        public LearningDirectorStatistics(DateTime startDate, List<EpochStatistics> learningEpochs, List<EpochStatistics> skilledEpochs) : base()
        {
            LearningStartDate = startDate;
            learningEpochs = learningEpochs.OrderBy(e => e.EpochOrder).ToList();
            skilledEpochs = skilledEpochs.OrderBy(e => e.EpochOrder).ToList();

            foreach (var epoch in learningEpochs)
                TotalMemories += epoch.MemoriesCount;

            foreach (var epoch in skilledEpochs)
            {
                TotalMemories += epoch.MemoriesCount;
                TrySetBestEpoch(epoch);
            }

            LearningEpochs = learningEpochs;
            SkilledEpochs = skilledEpochs;
        }

        public void UpdateCurrentTradeStatistics(BrokerSessionStatistics sessionStatistics)
        {
            CurrentEpoch.BrokerSessionStatistics = sessionStatistics;
        }

        public void RecordIteration(LearningAgentStatistics statistics)
        {
            TotalMemories += statistics.MemoriesCollected;

            CurrentEpoch.IterationsCount++; ;
            CurrentEpoch.EstimationsCount += statistics.QEstimations;
            CurrentEpoch.MemoriesCount += statistics.MemoriesCollected;
            CurrentEpoch.TotalIterationReward += statistics.TotalReward;
            CurrentEpoch.TotalLoss += statistics.TotalLoss;
        }

        public void RecordFinalEpsilon(double epsilon)
        {
            CurrentEpoch.FinalEpsilon = epsilon;
        }

        public void RecordSkilledEpoch()
        {
            SkilledEpochs.Add(CurrentEpoch);

            TrySetBestEpoch(CurrentEpoch);

            CreateNewEpoch();
        }

        public void RecordLearningEpoch()
        {
            LearningEpochs.Add(CurrentEpoch);

            CreateNewEpoch();
        }

        private bool IsBestEpoch(EpochStatistics epoch)
        {
            return epoch.BrokerSessionStatistics.BestTradedStock != null
                && (BestSkilledEpoch == null
                || BestSkilledEpoch.BrokerSessionStatistics.BestTradedStock.Profit < epoch.BrokerSessionStatistics.BestTradedStock.Profit);
        }

        private void TrySetBestEpoch(EpochStatistics epoch)
        {
            if (IsBestEpoch(epoch))
                BestSkilledEpoch = epoch;
        }

        private void CreateNewEpoch()
        {
            CurrentEpoch = new EpochStatistics();
            CurrentEpoch.EpochOrder = LearningEpochs.Count + SkilledEpochs.Count;
        }
    }
}
