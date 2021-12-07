using System;
using System.Collections.Generic;
using System.Text;
using AlgoTrading.SimulatedBroker;

namespace AlgoTrading.DQN
{
    public class DQNStatisticsManager
    {
        public DQNStatistics Statistics { get; private set; }

        public EpochStatistics CurrentEpochStatistics { get; private set; }

        public DQNStatisticsManager(DQNStatistics managableStatistics)
        {
            Statistics = managableStatistics;
            CurrentEpochStatistics = new EpochStatistics();
        }

        public void UpdateDQNMeta(DQNMeta meta)
        {
            meta.DQNStatistics = Statistics;
        }

        public void RecordEpoch()
        {
            if (CurrentEpochStatistics.AverageIterationReward > Statistics.RecordMaxIterationReward)
                Statistics.RecordMaxIterationReward = CurrentEpochStatistics.AverageIterationReward;

            Statistics.Epochs.Add(CurrentEpochStatistics);
            CurrentEpochStatistics = new EpochStatistics();

            Statistics.CurrentEpochID++;
        }

        public void UpdateEpochQStatistics(double estimatedQValue, double iterationReward, double totalLoss, double finalEpsilon, int estimationsCount, int iterationsCount)
        {
            CurrentEpochStatistics.IterationsCount += iterationsCount;
            CurrentEpochStatistics.EstimationsCount += estimationsCount;
            CurrentEpochStatistics.TotalIterationReward += iterationReward;
            CurrentEpochStatistics.TotalEstimatedQ += estimatedQValue;
            CurrentEpochStatistics.TotalLoss += totalLoss;
            CurrentEpochStatistics.FinalEpsilon = finalEpsilon;
        }

        public void GetEpochAverages()
        {
            CurrentEpochStatistics.AverageIterationReward = CurrentEpochStatistics.TotalIterationReward / CurrentEpochStatistics.IterationsCount;
            CurrentEpochStatistics.AverageEstimatedQ = CurrentEpochStatistics.TotalEstimatedQ / CurrentEpochStatistics.EstimationsCount;
            CurrentEpochStatistics.AverageLoss = CurrentEpochStatistics.TotalLoss / CurrentEpochStatistics.EstimationsCount;

            if(CurrentEpochStatistics.TotalTrades > 0)
            {
                CurrentEpochStatistics.AverageTradeDuration = CurrentEpochStatistics.TotalTradeDuration / CurrentEpochStatistics.TotalTrades;
                CurrentEpochStatistics.AverageTradeProfit = CurrentEpochStatistics.TotalCommissionedTradeProfit / CurrentEpochStatistics.TotalTrades;
            }
            else
            {
                CurrentEpochStatistics.AverageTradeDuration = 0;
                CurrentEpochStatistics.AverageTradeProfit = 0;
            }
                
        }

        public void UpdateEpochTradeStatistics(double tradeProfit, double tradeDuration, int tradeCount)
        {
            CurrentEpochStatistics.TotalTrades += tradeCount;
            CurrentEpochStatistics.TotalCommissionedTradeProfit += tradeProfit;
            CurrentEpochStatistics.TotalTradeDuration += tradeDuration;          
        }

        public void AddTotalMemories(int newMemories)
        {
            CurrentEpochStatistics.MemoriesCount += newMemories;
            Statistics.TotalMemories += newMemories;
        }
        
    }
}
