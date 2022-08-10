using AlgoTrading.Broker.Statistics;
using AlgoTrading.Statistics;

namespace AlgoTrading.DQN.Statistics
{
    public class EpochStatistics : IStatistics
    {
        public int EpochOrder { get; set; }
        public double AverageIterationReward
        {
            get
            {
                if (IterationsCount == 0)
                    return 0;
                else
                    return TotalIterationReward / IterationsCount;
            }
        }

        public double AverageLoss
        {
            get
            {
                if (EstimationsCount == 0)
                    return 0;
                else
                    return TotalLoss / EstimationsCount;
            }
        }

        public double TotalIterationReward { get; set; }
        public double TotalLoss { get; set; }
        public double FinalEpsilon { get; set; }
        public int MemoriesCount { get; set; }
        public int EstimationsCount { get; set; }
        public int IterationsCount { get; set; }

        public BrokerSessionStatistics BrokerSessionStatistics { get; set; }
    }
}
