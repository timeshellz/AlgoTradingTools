using System;
using System.Collections.Generic;
using System.Text;
using AlgoTrading.Broker;
using AlgoTrading.Statistics;

namespace AlgoTrading.DQN
{
    public class LearningDirectorStatistics : IStatistics
    {
        private double maxIterationReward = double.MinValue;
        private double maxSkilledReward = double.MinValue;

        public DateTime LearningStartTime { get; set; } = DateTime.Now;
        public int CurrentLearningEpochID { get; set; }       
        public int TotalMemories { get; set; }
        public List<EpochStatistics> LearningEpochs { get; set; } = new List<EpochStatistics>();
        public List<EpochStatistics> SkilledEpochs { get; set; } = new List<EpochStatistics>();
        public EpochStatistics BestSkilledEpoch { get; set; }
        public EpochStatistics CurrentEpoch { get; set; } = new EpochStatistics();

        public double MaxLearningReward 
        {
            get => maxIterationReward; 
            set
            {
                if (value > maxIterationReward)
                    maxIterationReward = value;
            }
        }

        public double MaxSkilledReward
        {
            get => maxSkilledReward;
            set
            {
                if (value > maxSkilledReward)
                    maxSkilledReward = value;
            }
        }

        //This method is booolsheeet
        public void UpdateCurrentQStatistics(double iterationReward, double totalLoss,
            double finalEpsilon, int estimationsCount, int iterationsCount, int memoriesCount)
        {
            CurrentEpoch.IterationsCount += iterationsCount;
            CurrentEpoch.EstimationsCount += estimationsCount;
            CurrentEpoch.MemoriesCount += memoriesCount;
            CurrentEpoch.TotalIterationReward += iterationReward;
            CurrentEpoch.TotalLoss += totalLoss;
            CurrentEpoch.FinalEpsilon = finalEpsilon;
        }

        public void UpdateCurrentTradeStatistics(BrokerSessionStatistics sessionStatistics)
        {
            CurrentEpoch.BrokerSessionStatistics = sessionStatistics;
        }       

        private void UpdateEpochAverages()
        {
            if(CurrentEpoch.IterationsCount != 0)
                CurrentEpoch.AverageIterationReward = CurrentEpoch.TotalIterationReward / CurrentEpoch.IterationsCount;

            if(CurrentEpoch.EstimationsCount != 0)
                CurrentEpoch.AverageLoss = CurrentEpoch.TotalLoss / CurrentEpoch.EstimationsCount;
        }

        public void RecordSkilledEpoch()
        {
            UpdateEpochAverages();

            MaxSkilledReward = CurrentEpoch.AverageIterationReward;

            SkilledEpochs.Add(CurrentEpoch);

            if (CurrentEpoch.BrokerSessionStatistics.BestTradedStock != null 
                && (BestSkilledEpoch == null || BestSkilledEpoch.BrokerSessionStatistics.BestTradedStock.Profit < CurrentEpoch.BrokerSessionStatistics.BestTradedStock.Profit))
                BestSkilledEpoch = CurrentEpoch;

            CurrentEpoch = new EpochStatistics();
        }

        public void RecordLearningEpoch()
        {
            UpdateEpochAverages();

            MaxLearningReward = CurrentEpoch.AverageIterationReward;

            LearningEpochs.Add(CurrentEpoch);
            CurrentEpoch = new EpochStatistics();

            CurrentLearningEpochID++;
        }
    }
}
