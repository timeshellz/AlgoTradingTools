using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AlgoTrading.Stocks.Persistence.Database
{
    public class NeuralDataContext : DbContext
    {
        public NeuralDataContext(DbContextOptions<NeuralDataContext> options) : base(options)
        {
        }

        public DbSet<StockBarDTO> Bars { get; set; }
        public DbSet<StockDataDTO> Stocks { get; set; }
        public DbSet<StockInfoDTO> StockInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockBarDTO>().ToTable("Bars");
            modelBuilder.Entity<StockDataDTO>().ToTable("Stocks");
            modelBuilder.Entity<StockInfoDTO>().ToTable("StockInfos");

            modelBuilder.Entity<StockDataDTO>()
                .HasKey(nameof(StockDataDTO.FIGI), nameof(StockDataDTO.Interval));
        }

    }
}