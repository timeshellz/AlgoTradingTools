using AlgoTrading.DQN;
using AlgoTrading.Neural;
using AlgoTrading.Neural.Persistence;
using AlgoTrading.Agent.Learning;
using AlgoTrading.Stocks;
using AlgoTrading.Stocks.Persistence;
using AlgoTrading.Broker;
using AlgoTrading.Broker.Simulated;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace TradeBubble.Services
{
    public class LearningManagerService : BackgroundService
    {
        public static event EventHandler<LearningStatisticsUpdatedEventArgs> StatisticsUpdated;

        public static LearningDirectorConfiguration DirectorConfiguration { get; set; }

        public static bool AreSettingsReady { get; set; } = false;

        public static bool IsPaused 
        { 
            get => isPaused; 
            set
            {               
                if (director != null)
                {
                    //if (isPaused)
                    //    controller.ControllerPauseEvent.Set();
                    //else
                    //    controller.ControllerPauseEvent.Reset();

                    isPaused = value;
                }                  
            }
        }

        private static LearningAgentDirector director;
        private static bool isPaused = true;

        private IServiceProvider serviceProvider;

        public LearningManagerService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await RunDQNSetup();
                await RunDirector();
            }
        }

        private async Task RunDQNSetup()
        {
            while(director == null)
            {
                try
                {
                    while (!AreSettingsReady || DirectorConfiguration == null)
                        await Task.Delay(100);

                    await CreateLearningDirector();
                }
                catch
                {
                    director = null;
                    AreSettingsReady = false;
                    //log error message
                }
            }           
        }

        private async Task CreateLearningDirector()
        {
            DirectorConfiguration.NeuralConfiguration.Inputs = MarketState.GetPossibleValues();
            DirectorConfiguration.NeuralConfiguration.Outputs = Enum.GetValues(typeof(BrokerAction))
                .Cast<BrokerAction>().Select(v => v.GetActionString()).ToList();

            IBroker broker = new BrokerEmulator(DirectorConfiguration.BrokerConfiguration,
                (IStockPersistenceManager)serviceProvider.GetService(typeof(IStockPersistenceManager)),
                (IIndicatorProvider)serviceProvider.GetService(typeof(IIndicatorProvider)));

            await broker.Start();
           
            NeuralNetwork onlineNetwork = new NeuralNetwork("test", DirectorConfiguration.NeuralConfiguration);
            NeuralNetwork targetNetwork = new NeuralNetwork("targetTest", DirectorConfiguration.NeuralConfiguration);
            targetNetwork.PasteWeights(onlineNetwork.CopyWeights());

            LearningAgent agent = new LearningAgent(DirectorConfiguration.AgentConfiguration,
                targetNetwork, onlineNetwork, broker);

            director = new LearningAgentDirector(agent);
        }

        private async Task RunDirector()
        {
            int trainingEpochsElapsed = 0;

            while(true)
            {
                while (IsPaused)
                    await Task.Delay(100);

                if(trainingEpochsElapsed != 5)
                {
                    await director.DirectLearningEpoch();
                    trainingEpochsElapsed++;
                }
                else
                {
                    director.Agent.UpdateTargetNetwork();
                    trainingEpochsElapsed = 0;

                    var bestResult = director.GetStatistics().BestSkilledEpoch;
                    await director.DirectSkilledEpoch();

                    if (director.GetStatistics().BestSkilledEpoch != bestResult)
                        await SaveNetwork(director.Agent.TargetNetwork);
                }

                StatisticsUpdated?.Invoke(this, new LearningStatisticsUpdatedEventArgs(director.GetStatistics()));
            }
            
        }

        private async Task SaveNetwork(NeuralNetwork network)
        {
            INeuralPersistenceManager persistenceManager = (INeuralPersistenceManager)serviceProvider.GetService(typeof(INeuralPersistenceManager));

            await persistenceManager.SaveNeuralNetwork(network);
        }
    }

    public class LearningStatisticsUpdatedEventArgs : EventArgs
    {
        public LearningDirectorStatistics Statistics { get; }

        public LearningStatisticsUpdatedEventArgs(LearningDirectorStatistics statistics)
        {
            Statistics = statistics;
        }
    }
}
