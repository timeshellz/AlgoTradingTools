using AlgoTrading.Broker.Statistics.Persistence.Database;
using AlgoTrading.Broker.Statistics.Persistence.Database.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgoTrading.DQN.Statistics.Persistence.Database.DTO
{
    public class LearningSessionStatisticsDTO
    {
        public Guid Id { get; set; }
        public DateTime LearningStartDate { get; set; }
        public int TotalMemories { get; set; }
        public List<EpochStatisticsDTO> Epochs { get; set; }
    }

    public enum EpochType { Learning, Skilled }

    public class EpochStatisticsDTO
    {
        public int Id { get; set; }
        public int EpochOrder { get; set; }
        public double TotalIterationReward { get; set; }
        public double TotalLoss { get; set; }
        public double FinalEpsilon { get; set; }
        public int MemoriesCount { get; set; }
        public int EstimationsCount { get; set; }
        public int IterationsCount { get; set; }
        public EpochType Type { get; set; }
        public BrokerSessionStatisticsDTO BrokerSessionStatistics { get; set; }
    }

    public static class DQNStatisticsModelExtensions
    {
        public static LearningSessionStatisticsDTO GetDTO(this LearningDirectorStatistics statistics)
        {
            var epochs = statistics.SkilledEpochs.Select(e => e.GetDTO(EpochType.Skilled));
            epochs = epochs.Concat(statistics.LearningEpochs.Select(e => e.GetDTO(EpochType.Learning)));

            return new LearningSessionStatisticsDTO()
            {
                Id = statistics.UUID,
                Epochs = epochs.ToList(),
                LearningStartDate = statistics.LearningStartDate,
                TotalMemories = statistics.TotalMemories,
            };
        }

        public static EpochStatisticsDTO GetDTO(this EpochStatistics statistics, EpochType type)
        {
            return new EpochStatisticsDTO()
            {
                EpochOrder = statistics.EpochOrder,
                EstimationsCount = statistics.EstimationsCount,
                IterationsCount = statistics.IterationsCount,
                MemoriesCount = statistics.MemoriesCount,
                TotalIterationReward = statistics.TotalIterationReward,
                TotalLoss = statistics.TotalLoss,
                Type = type,
                BrokerSessionStatistics = statistics.BrokerSessionStatistics.GetDTO(),
                FinalEpsilon = statistics.FinalEpsilon,
            };
        }

        public static LearningDirectorStatistics GetModel(this LearningSessionStatisticsDTO statisticsDTO)
        {
            if (statisticsDTO.Epochs != null && statisticsDTO.Epochs.Count > 0)
            {
                var model = new LearningDirectorStatistics(statisticsDTO.LearningStartDate,
                    statisticsDTO.Epochs.Where(e => e.Type == EpochType.Learning).Select(e => e.GetModel()).ToList(),
                    statisticsDTO.Epochs.Where(e => e.Type == EpochType.Skilled).Select(e => e.GetModel()).ToList());

                model.UUID = statisticsDTO.Id;
                return model;
            }

            return new LearningDirectorStatistics();
        }

        public static EpochStatistics GetModel(this EpochStatisticsDTO statisticsDTO)
        {
            return new EpochStatistics()
            {
                EpochOrder = statisticsDTO.EpochOrder,
                EstimationsCount = statisticsDTO.EstimationsCount,
                FinalEpsilon = statisticsDTO.FinalEpsilon,
                IterationsCount = statisticsDTO.IterationsCount,
                MemoriesCount = statisticsDTO.MemoriesCount,
                TotalIterationReward = statisticsDTO.TotalIterationReward,
                TotalLoss = statisticsDTO.TotalLoss,
                BrokerSessionStatistics = statisticsDTO.BrokerSessionStatistics.GetModel()
            };
        }
    }
}
