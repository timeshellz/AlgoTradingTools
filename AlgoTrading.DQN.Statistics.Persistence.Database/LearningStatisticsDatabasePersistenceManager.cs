using AlgoTrading.DQN.Statistics.Persistence.Database.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoTrading.DQN.Statistics.Persistence.Database
{
    public class LearningStatisticsDatabasePersistenceManager : ILearningStatisticsPersistenceManager
    {
        IDbContextFactory<LearningStatisticsDataContext> dbFactory;

        public LearningStatisticsDatabasePersistenceManager(IDbContextFactory<LearningStatisticsDataContext> factory)
        {
            dbFactory = factory;
        }

        public async Task<LearningDirectorStatistics> LoadStatistics(Guid guid)
        {
            try
            {
                using (var db = dbFactory.CreateDbContext())
                {
                    var existingStatistics = await db.LearningStatistics
                        .Include(s => s.Epochs)
                        .ThenInclude(e => e.BrokerSessionStatistics)
                        .ThenInclude(e => e.BestTradedStock)
                        .ThenInclude(e => e.Positions)
                        .FirstOrDefaultAsync(s => s.Id == guid);

                    return existingStatistics.GetModel();
                }
            }
            catch (Exception e)
            { }

            return null;
        }

        public async Task SaveStatistics(LearningDirectorStatistics statistics)
        {
            try
            {
                using (var db = dbFactory.CreateDbContext())
                {
                    var existingStatistics = await db.LearningStatistics
                        .Include(s => s.Epochs)
                        .ThenInclude(e => e.BrokerSessionStatistics)
                        .ThenInclude(e => e.BestTradedStock)
                        .ThenInclude(e => e.Positions)
                        .FirstOrDefaultAsync(s => s.Id == statistics.UUID);

                    var newStatistics = statistics.GetDTO();

                    if (existingStatistics != null)
                    {
                        db.Entry(existingStatistics).CurrentValues.SetValues(newStatistics);

                        existingStatistics.Epochs.AddRange(newStatistics.Epochs.Where(e => !existingStatistics.Epochs.Any(ex => ex.EpochOrder == e.EpochOrder)));

                        db.LearningStatistics.Update(existingStatistics);
                    }
                    else
                        db.LearningStatistics.Add(newStatistics);

                    await db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            { }
        }
    }
}
