using System;
using System.Collections.Generic;
using System.Text;
using AlgoTrading.Broker;
using AlgoTrading.Statistics;

namespace AlgoTrading.DQN
{
    public class EpochStatistics : IStatistics
    {
        static int lastEpochID = 0;
        public int EpochID { get; set; } = lastEpochID++;
        public double AverageIterationReward { get; set; }
        public double AverageLoss { get; set; }
        public double TotalIterationReward { get; set; }
        public double TotalLoss { get; set; }
        public double FinalEpsilon { get; set; }
        public int MemoriesCount { get; set; } = 0;
        public int EstimationsCount { get; set; } = 0;
        public int IterationsCount { get; set; } = 0;

        public BrokerSessionStatistics BrokerSessionStatistics { get; set; }
    }
}
