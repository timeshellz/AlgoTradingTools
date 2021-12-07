using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using AlgoTrading.DataKeeping;
using Cyotek.Collections.Generic;
using AlgoTrading.Neural;
using AlgoTrading.SimulatedBroker;

namespace AlgoTrading.DQN
{
    public class PrimaryDQNController
    {       
        public ManualResetEvent ControllerPauseEvent { get; private set; }

        public DQNStatisticsManager StatisticsManager { get; private set; }
        public NeuralNetwork NeuralNetwork { get; private set; }
        //public NeuralNetwork CriticNeuralNetwork { get; private set; }
        public AgentFileManager AgentSaveManager { get; private set; }
        public NeuralNetworkFileManager NeuralSaveManager { get; private set; }
        //public NeuralNetworkFileManager CriticNeuralSaveManager { get; private set; }
        public DQNMeta DQNMeta { get; private set; }
        public DQNStatistics Statistics { get; private set; }
        public Agent Agent { get; private set; }

        public PrimaryDQNController(DQNMeta dqnMeta)
        {
            NeuralSaveManager = new NeuralNetworkFileManager(dqnMeta.NetworkName);
            //CriticNeuralSaveManager = new NeuralNetworkFileManager(dqnMeta.NetworkName + "-critic");
            AgentSaveManager = new AgentFileManager(dqnMeta.AgentName);
            Statistics = new DQNStatistics();
            StatisticsManager = new DQNStatisticsManager(Statistics);

            DQNMeta = dqnMeta;

            Initialize();
        }        

        void Initialize()
        {
            try
            {
                NeuralNetwork = NeuralSaveManager.LoadNetwork();
            }
            catch
            {
                NeuralNetwork = new NeuralNetwork(DQNMeta.NetworkName, DQNMeta.NeuralSettings);
                //NeuralSaveManager.SaveNetwork(NeuralNetwork);
            }

            /*try
            {
                CriticNeuralNetwork = CriticNeuralSaveManager.LoadNetwork();
            }
            catch
            {
                List<string> criticInputs = new List<string>(DQNMeta.NeuralSettings.Inputs);
                criticInputs.Add("Action");

                NeuralSettings criticSettings = new NeuralSettings(DQNMeta.NeuralSettings.NetworkName + "-critic",
                    criticInputs, new List<string>() { "Advantage" }, DQNMeta.NeuralSettings.HiddenLayerCount, DQNMeta.NeuralSettings.LearningRate);

                CriticNeuralNetwork = new NeuralNetwork(DQNMeta.NetworkName, criticSettings);
               // CriticNeuralSaveManager.SaveNetwork(CriticNeuralNetwork);
            }*/

            try
            {
                Agent = AgentSaveManager.LoadAgent();
            }
            catch
            {
                Agent = new Agent(DQNMeta.AgentSettings);
                AgentSaveManager.SaveAgent(Agent);
            }

            Agent.SetOnlineNetwork(new NeuralNetwork(NeuralNetwork.Name + "-online", NeuralNetwork.Settings));
            Agent.SetTargetNetwork(NeuralNetwork);
            Agent.OnlineNetwork.PasteWeights(NeuralNetwork.CopyWeights());

            /*Agent.SetCriticOnlineNetwork(new NeuralNetwork(CriticNeuralNetwork.Name + "-online", CriticNeuralNetwork.Settings));
            Agent.SetCriticTargetNetwork(CriticNeuralNetwork);
            Agent.OnlineCriticNetwork.PasteWeights(CriticNeuralNetwork.CopyWeights());*/

            Agent.SetEmulator(CreateBrokerEmulator());
        }

        BrokerEmulator CreateBrokerEmulator()
        {
            BrokerEmulator brokerEmulator = new BrokerEmulator(DQNMeta.BrokerSettings);
            return brokerEmulator;
        }

        public void RunDQN()
        {
            ControllerPauseEvent = new ManualResetEvent(false);

            while(true)
            {
                ControllerPauseEvent.WaitOne();

                IterationStatistics iterationStats = Agent.RunIteration();
                Agent.UpdateTargetActorNetwork();
                //Agent.UpdateTargetCriticNetwork();

                StatisticsManager.UpdateEpochQStatistics(iterationStats.TotalEstimatedQ, iterationStats.TotalReward, iterationStats.TotalLoss,
                    Agent.Settings.CurrentEpsilon, iterationStats.EstimationCount, 1);
                StatisticsManager.AddTotalMemories(iterationStats.CollectedMemoriesCount);

                StatisticsManager.UpdateEpochTradeStatistics(iterationStats.TotalCommisionedTradeProfit, iterationStats.TotalTradeDuration, iterationStats.TradeCount);

                if (iterationStats.IsEpochOver)
                {
                    StatisticsManager.GetEpochAverages();
                    StatisticsManager.RecordEpoch();
                    StatisticsManager.UpdateDQNMeta(DQNMeta);
                }                
            }                      
        }

        public void SaveNetwork()
        {
            if (AgentSaveManager == null)
                throw new NullReferenceException();

            NeuralSaveManager.SaveNetwork(NeuralNetwork);
            AgentSaveManager.SaveAgent(Agent);
        }        
    }
}
