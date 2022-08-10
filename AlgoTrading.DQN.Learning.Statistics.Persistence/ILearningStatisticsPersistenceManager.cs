using System;
using System.Threading.Tasks;

namespace AlgoTrading.DQN.Statistics.Persistence
{
    public interface ILearningStatisticsPersistenceManager
    {
        Task<LearningDirectorStatistics> LoadStatistics(Guid guid);
        Task SaveStatistics(LearningDirectorStatistics statistics);
    }
}
