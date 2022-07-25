using System;
using System.Collections.Generic;
using System.Text;
using AlgoTrading.Statistics;

namespace AlgoTrading.Agent.Learning
{
    public class LearningAgentStatistics : IStatistics
    {
        public int MemoriesCollected { get; set; }
        public double TotalReward { get; set; }
        public int InteractionsCount { get; set; }
        public int QEstimations { get; set; }
        public double TotalLoss { get; set; }

        public void RecordInteraction(double interactionReward, int memoriesCollected)
        {
            InteractionsCount++;
            MemoriesCollected += memoriesCollected;
            TotalReward += interactionReward;
        }

        public void RecordLossCalculation(double addedLoss)
        {
            TotalLoss += addedLoss;
        }

        public void RecordQEstimation()
        {
            QEstimations++;
        }
    }
}
