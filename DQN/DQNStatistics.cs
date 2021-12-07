using System;
using System.Collections.Generic;
using System.Text;
using AlgoTrading.SimulatedBroker;

namespace AlgoTrading.DQN
{
    public class DQNStatistics
    {
        public DateTime DQNStartTime { get; set; }
        public int CurrentEpochID { get; set; }
        public double RecordMaxIterationReward { get; set; }
        public int TotalMemories { get; set; }
        public List<EpochStatistics> Epochs { get; set; }

        public DQNStatistics()
        {
            DQNStartTime = DateTime.Now;
            CurrentEpochID = 0;
            RecordMaxIterationReward = double.MinValue;
            TotalMemories = 0;
            Epochs = new List<EpochStatistics>();
        }
    }
}
