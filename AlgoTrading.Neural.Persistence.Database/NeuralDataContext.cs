using AlgoTrading.Neural.Persistence.Database.DTO;
using Microsoft.EntityFrameworkCore;

namespace AlgoTrading.Neural.Persistence.Database
{
    public class NeuralDataContext : DbContext
    {
        public NeuralDataContext(DbContextOptions<NeuralDataContext> options) : base(options)
        {
        }

        public DbSet<NeuralNetworkDTO> NeuralNetworks { get; set; }
        public DbSet<NeuralConfigurationDTO> NeuralNetworkConfigurations { get; set; }
        public DbSet<NodeConnectionDTO> NodeConnections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NeuralNetworkDTO>().ToTable("NeuralNetworks");
            modelBuilder.Entity<NodeDTO>().ToTable("Nodes");
            modelBuilder.Entity<InputNodeDTO>().ToTable("InputNodes");
            modelBuilder.Entity<NeuronDTO>().ToTable("Neurons");
            modelBuilder.Entity<OutputNeuronDTO>().ToTable("OutputNeurons");
            modelBuilder.Entity<NodeConnectionDTO>().ToTable("NodeConnections");
            modelBuilder.Entity<NeuralConfigurationDTO>().ToTable("NeuralNetworkConfigurations");

            modelBuilder.Entity<NodeConnectionDTO>()
                .HasOne(r => r.InputNode)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NodeConnectionDTO>()
                .HasOne(r => r.OutputNode)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}