using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Threading.Tasks;
using AlgoTrading.DQN;
using AlgoTrading.Neural;
using AlgoTrading.SimulatedBroker;
using AlgoTrading.Stocks;

namespace AlgoTrading.DQN.UserInterface
{
    public class DQNMainPresenter : INotifyPropertyChanged
    {
        public event EventHandler SettingsReady;
        public event EventHandler StocksReady;
        public event EventHandler ControllerThreadReady;
        public event PropertyChangedEventHandler PropertyChanged;

        public BrokerSettings BrokerSettings { get; private set; }
        public NeuralSettings NeuralSettings { get; private set; }
        public AgentSettings AgentSettings { get; private set; }
        public List<string> LoadedStocksStrings { get; private set; }

        public NeuralNetworkFileManager NeuralSaveManager { get; private set; }
        public AgentFileManager AgentSaveManager { get; private set; }

        public DQNMeta DQNMeta { get; private set; }
        public DQNMetaFileManager MetaSaveManager { get; private set; }
        public PrimaryDQNController DQNController { get; private set; }

        Thread ControllerThread;

        bool isControllerRunning = false;
        public bool IsControllerRunning
        {
            get
            {
                return isControllerRunning;
            }
            set
            {
                isControllerRunning = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsControllerRunning)));
            }
        }

        public void SetupNewDQN(BrokerSettings brokerSettings, NeuralSettings neuralSettings, AgentSettings agentSettings)
        {
            SetDQNSettings(brokerSettings, neuralSettings, agentSettings);

            CreateDQNMeta();
            SaveDQNMeta();

            InitializeDQNController();
        }

        public void LoadOldDQN(string dqnMetaFileName)
        {
            MetaSaveManager = new DQNMetaFileManager(dqnMetaFileName);
            DQNMeta = MetaSaveManager.LoadMeta();

            SetDQNSettings(DQNMeta);

            InitializeDQNController();
        }

        void InitializeDQNController()
        {
            DQNController = new PrimaryDQNController(DQNMeta);

            StartDQNThread();
        }

        void StartDQNThread()
        {
            ControllerThread = new Thread(DQNController.RunDQN);
            ControllerThread.Start();

            ControllerThreadReady.Invoke(this, new EventArgs());
        }

        public void ControllerStartStop()
        {
            if (DQNController == null)
                return;

            if (IsControllerRunning)
                DQNController.ControllerPauseEvent.Reset();
            else
                DQNController.ControllerPauseEvent.Set();

            IsControllerRunning = !IsControllerRunning;
        }

        public void SetStockData(List<string> stockDataStrings)
        {
            LoadedStocksStrings = stockDataStrings;

            StocksReady.Invoke(this, new EventArgs());
        }

        void CreateDQNMeta()
        {
            DQNMeta = new DQNMeta(NeuralSettings.NetworkName, NeuralSaveManager.FileName,
                AgentSaveManager.FileName, AgentSettings, NeuralSettings, BrokerSettings);

            MetaSaveManager = new DQNMetaFileManager(DQNMeta);
        }

        void SaveDQNMeta()
        {
            MetaSaveManager.SaveMeta();
        }

        void SetDQNSettings(DQNMeta dQNMeta)
        {
            BrokerSettings = dQNMeta.BrokerSettings;
            NeuralSettings = dQNMeta.NeuralSettings;
            AgentSettings = dQNMeta.AgentSettings;
            LoadedStocksStrings = BrokerSettings.StockMetas;
            
            SettingsReady.Invoke(this, new EventArgs());
            StocksReady.Invoke(this, new EventArgs());

            SetSaveManagers();
        }

        void SetDQNSettings(BrokerSettings brokerSettings, NeuralSettings neuralSettings, AgentSettings agentSettings)
        {
            BrokerSettings = brokerSettings;
            NeuralSettings = neuralSettings;
            AgentSettings = agentSettings;

            SettingsReady.Invoke(this, new EventArgs());

            SetSaveManagers();
        }

        void SetSaveManagers()
        {
            NeuralSaveManager = new NeuralNetworkFileManager(NeuralSettings.NetworkName);
            AgentSaveManager = new AgentFileManager(AgentSettings.AgentName);
        }

        
    }
}
