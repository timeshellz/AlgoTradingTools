using System;
using System.Collections.Generic;
using System.Text;
using AlgoTrading.SimulatedBroker;

namespace AlgoTrading.DQN
{
    public class EpochStatistics
    {
        static int lastEpochID = 0;
        public int EpochID { get; set; } = lastEpochID++;
        public double AverageIterationReward { get; set; }
        public double AverageEstimatedQ { get; set; }
        public double AverageLoss { get; set; }
        public double TotalIterationReward { get; set; }
        public double TotalEstimatedQ { get; set; }
        public double TotalLoss { get; set; }
        public double FinalEpsilon { get; set; }
        public int MemoriesCount { get; set; } = 0;
        public int EstimationsCount { get; set; } = 0;
        public int IterationsCount { get; set; } = 0;

        public double TotalCommissionedTradeProfit { get; set; }
        public double TotalTradeDuration { get; set; }
        public int TotalTrades { get; set; }
        public double AverageTradeProfit { get; set; }
        public double AverageTradeDuration { get; set; }        
    }
}
