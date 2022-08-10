using AlgoTrading.DQN.Statistics.Persistence.Database.DTO;
using AlgoTrading.Broker.Statistics.Persistence.Database.DTO;
using Microsoft.EntityFrameworkCore;

namespace AlgoTrading.DQN.Statistics.Persistence.Database
{
    public class LearningStatisticsDataContext : DbContext
    {
        public LearningStatisticsDataContext(DbContextOptions<LearningStatisticsDataContext> options) : base(options)
        {
        }

        public DbSet<LearningSessionStatisticsDTO> LearningStatistics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LearningSessionStatisticsDTO>().ToTable("LearningStatistics");
            modelBuilder.Entity<TradedStockStatisticsDTO>().ToTable("TradedStockStatistics");
            modelBuilder.Entity<BrokerSessionStatisticsDTO>().ToTable("BrokerSessionStatistics");
            modelBuilder.Entity<MarketPositionStatisticsDTO>().ToTable("MarketPositionStatistics");
            modelBuilder.Entity<EpochStatisticsDTO>().ToTable("EpochStatistics");
        }
    }
}
