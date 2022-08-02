﻿// <auto-generated />
using System;
using AlgoTrading.Neural.Persistence.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace AlgoTrading.Neural.Persistence.Database.Migrations
{
    [DbContext(typeof(NeuralDataContext))]
    partial class NeuralDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.16")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("AlgoTrading.Neural.Persistence.Database.DTO.NeuralConfigurationDTO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("HiddenLayerCount")
                        .HasColumnType("integer");

                    b.Property<string>("NetworkName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("NeuralNetworkConfigurations");
                });

            modelBuilder.Entity("AlgoTrading.Neural.Persistence.Database.DTO.NeuralNetworkDTO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ConfigurationId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ConfigurationId");

                    b.ToTable("NeuralNetworks");
                });

            modelBuilder.Entity("AlgoTrading.Neural.Persistence.Database.DTO.NodeConnectionDTO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("InputNodeId")
                        .HasColumnType("integer");

                    b.Property<int>("LocalId")
                        .HasColumnType("integer");

                    b.Property<int?>("NeuralNetworkDTOId")
                        .HasColumnType("integer");

                    b.Property<int?>("OutputNodeId")
                        .HasColumnType("integer");

                    b.Property<double>("Weight")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("InputNodeId");

                    b.HasIndex("NeuralNetworkDTOId");

                    b.HasIndex("OutputNodeId");

                    b.ToTable("NodeConnections");
                });

            modelBuilder.Entity("AlgoTrading.Neural.Persistence.Database.DTO.NodeDTO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Layer")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Nodes");
                });

            modelBuilder.Entity("AlgoTrading.Neural.Persistence.Database.DTO.InputNodeDTO", b =>
                {
                    b.HasBaseType("AlgoTrading.Neural.Persistence.Database.DTO.NodeDTO");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("NeuralNetworkDTOId")
                        .HasColumnType("integer");

                    b.HasIndex("NeuralNetworkDTOId");

                    b.ToTable("InputNodes");
                });

            modelBuilder.Entity("AlgoTrading.Neural.Persistence.Database.DTO.NeuronDTO", b =>
                {
                    b.HasBaseType("AlgoTrading.Neural.Persistence.Database.DTO.NodeDTO");

                    b.Property<int>("ActivationType")
                        .HasColumnType("integer");

                    b.Property<int?>("NeuralNetworkDTOId")
                        .HasColumnType("integer");

                    b.HasIndex("NeuralNetworkDTOId");

                    b.ToTable("Neurons");
                });

            modelBuilder.Entity("AlgoTrading.Neural.Persistence.Database.DTO.OutputNeuronDTO", b =>
                {
                    b.HasBaseType("AlgoTrading.Neural.Persistence.Database.DTO.NeuronDTO");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("NeuralNetworkDTOId1")
                        .HasColumnType("integer");

                    b.HasIndex("NeuralNetworkDTOId1");

                    b.ToTable("OutputNeurons");
                });

            modelBuilder.Entity("AlgoTrading.Neural.Persistence.Database.DTO.NeuralNetworkDTO", b =>
                {
                    b.HasOne("AlgoTrading.Neural.Persistence.Database.DTO.NeuralConfigurationDTO", "Configuration")
                        .WithMany()
                        .HasForeignKey("ConfigurationId");

                    b.Navigation("Configuration");
                });

            modelBuilder.Entity("AlgoTrading.Neural.Persistence.Database.DTO.NodeConnectionDTO", b =>
                {
                    b.HasOne("AlgoTrading.Neural.Persistence.Database.DTO.NodeDTO", "InputNode")
                        .WithMany()
                        .HasForeignKey("InputNodeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("AlgoTrading.Neural.Persistence.Database.DTO.NeuralNetworkDTO", null)
                        .WithMany("Connections")
                        .HasForeignKey("NeuralNetworkDTOId");

                    b.HasOne("AlgoTrading.Neural.Persistence.Database.DTO.NodeDTO", "OutputNode")
                        .WithMany()
                        .HasForeignKey("OutputNodeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("InputNode");

                    b.Navigation("OutputNode");
                });

            modelBuilder.Entity("AlgoTrading.Neural.Persistence.Database.DTO.InputNodeDTO", b =>
                {
                    b.HasOne("AlgoTrading.Neural.Persistence.Database.DTO.NodeDTO", null)
                        .WithOne()
                        .HasForeignKey("AlgoTrading.Neural.Persistence.Database.DTO.InputNodeDTO", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AlgoTrading.Neural.Persistence.Database.DTO.NeuralNetworkDTO", null)
                        .WithMany("Inputs")
                        .HasForeignKey("NeuralNetworkDTOId");
                });

            modelBuilder.Entity("AlgoTrading.Neural.Persistence.Database.DTO.NeuronDTO", b =>
                {
                    b.HasOne("AlgoTrading.Neural.Persistence.Database.DTO.NodeDTO", null)
                        .WithOne()
                        .HasForeignKey("AlgoTrading.Neural.Persistence.Database.DTO.NeuronDTO", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AlgoTrading.Neural.Persistence.Database.DTO.NeuralNetworkDTO", null)
                        .WithMany("Neurons")
                        .HasForeignKey("NeuralNetworkDTOId");
                });

            modelBuilder.Entity("AlgoTrading.Neural.Persistence.Database.DTO.OutputNeuronDTO", b =>
                {
                    b.HasOne("AlgoTrading.Neural.Persistence.Database.DTO.NeuronDTO", null)
                        .WithOne()
                        .HasForeignKey("AlgoTrading.Neural.Persistence.Database.DTO.OutputNeuronDTO", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AlgoTrading.Neural.Persistence.Database.DTO.NeuralNetworkDTO", null)
                        .WithMany("Outputs")
                        .HasForeignKey("NeuralNetworkDTOId1");
                });

            modelBuilder.Entity("AlgoTrading.Neural.Persistence.Database.DTO.NeuralNetworkDTO", b =>
                {
                    b.Navigation("Connections");

                    b.Navigation("Inputs");

                    b.Navigation("Neurons");

                    b.Navigation("Outputs");
                });
#pragma warning restore 612, 618
        }
    }
}
