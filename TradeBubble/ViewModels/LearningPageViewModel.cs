using System;
using AlgoTrading.Broker;
using AlgoTrading.Neural;
using AlgoTrading.Agent.Learning;
using AlgoTrading.Stocks;
using AlgoTrading.DQN;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeBubble.Services;

namespace TradeBubble.ViewModels
{
    public class LearningPageViewModel : PageViewModel
    {
        private LearningManagerService learningManager;

        private SetupState currentSetupState = SetupState.None;
        private bool isCreatingDQN = false;

        public Dictionary<DataInterval, List<StockIdentifier>> AvailableStocks { get; set; } = new Dictionary<DataInterval, List<StockIdentifier>>();
        public List<IntervalStockIdentifier> DisplayedStocks { get; set; } = new List<IntervalStockIdentifier>();
        public List<IntervalStockIdentifier> SelectedStocks { get; set; } = new List<IntervalStockIdentifier>();

        public Dictionary<int, EpochStatistics> LearningEpochs { get; set; } = new Dictionary<int, EpochStatistics>();
        public Dictionary<int, EpochStatistics> SkilledEpochs { get; set; } = new Dictionary<int, EpochStatistics>();
        public Dictionary<int, BrokerSessionStatistics> BrokerSessionStatistics1 { get; set; } = new Dictionary<int, BrokerSessionStatistics>();

        public enum SetupState { LoadSaved, CreateNew, SelectStocks, None };        

        public LearningAgentConfiguration AgentConfiguration { get; set; }
        public NeuralConfiguration NeuralConfiguration { get; set; }
        public BrokerConfiguration BrokerConfiguration { get; set; }

        public TradedStockStatistics BestTradedStock { get; set; }


        public bool IsConfigured { get; set; } = false || ( LearningManagerService.AreSettingsReady);
       
        public SetupState CurrentSetupState 
        {
            get => currentSetupState;
            private set
            {
                currentSetupState = value;

                if (currentSetupState == SetupState.SelectStocks)
                    StockDataService.FetchSavedStocks();
            }
        }             

        public bool IsCreatingDQN
        {
            get => isCreatingDQN;
            set
            {
                isCreatingDQN = value;

                if(value)
                {
                    CurrentSetupState = SetupState.CreateNew;

                    AgentConfiguration = new LearningAgentConfiguration();
                    NeuralConfiguration = new NeuralConfiguration();
                    BrokerConfiguration = new BrokerConfiguration();
                }    

                OnPropertyChanged();
            }
        }

        public LearningPageViewModel(LearningManagerService learningManagerService)
        {
            learningManager = learningManagerService;
        }

        public bool IsLearningRunning 
        {
            get
            {
                return !LearningManagerService.IsPaused;
            } 
            set
            {
                LearningManagerService.IsPaused = !value;

                OnPropertyChanged();
            }
        }

        protected override async Task LoadData()
        {
            IsLearningRunning = !LearningManagerService.IsPaused;
            StockDataService.StockDataFetched += StockDataFetched;
            LearningManagerService.StatisticsUpdated += StatisticsUpdated;
        }

        private void StockDataFetched(object sender, StockDataFetchedEventArgs e)
        {
            if (e.Type == DataType.Saved)
            {
                if (!AvailableStocks.ContainsKey(e.Interval))
                    AvailableStocks.Add(e.Interval, new List<StockIdentifier>());

                AvailableStocks[e.Interval] = e.StockIdentifiers;

                DisplayedStocks = AvailableStocks
                    .SelectMany(e => e.Value.Select(v => new IntervalStockIdentifier(e.Key, v))).ToList();
            }

            OnPropertyChanged();
        }

        private void StatisticsUpdated(object sender, LearningStatisticsUpdatedEventArgs e)
        {
            LearningEpochs = e.Statistics.LearningEpochs.ToDictionary(k => k.EpochID);
            SkilledEpochs = e.Statistics.SkilledEpochs.ToDictionary(k => k.EpochID);

            BrokerSessionStatistics1 = LearningEpochs
                .Select(e => new KeyValuePair<int, BrokerSessionStatistics>(e.Key, e.Value.BrokerSessionStatistics))
                .ToDictionary(k => k.Key, v => v.Value);

            if(SkilledEpochs.Count > 0)
                BestTradedStock = SkilledEpochs.Last().Value.BrokerSessionStatistics.BestTradedStock;

            OnPropertyChanged();
        }

        private void ConfirmSetup()
        {
            BrokerConfiguration.StockNames = SelectedStocks.Select(e => e.Identifier).Distinct().ToList();

            LearningManagerService.DirectorConfiguration = new LearningDirectorConfiguration(AgentConfiguration, NeuralConfiguration, BrokerConfiguration);

            IsCreatingDQN = false;
            IsConfigured = true;
            LearningManagerService.AreSettingsReady = true;
        }

        public void AdvanceSetup()
        {
            if (CurrentSetupState == SetupState.CreateNew)
                CurrentSetupState = SetupState.SelectStocks;
            else if(CurrentSetupState != SetupState.None)
                ConfirmSetup();
        }

        public void BacktrackSetup()
        {
            if (CurrentSetupState == SetupState.None)
                return;

            if (CurrentSetupState == SetupState.CreateNew)
            {
                CurrentSetupState = SetupState.None;
                IsCreatingDQN = false;

                AgentConfiguration = null;
                BrokerConfiguration = null;
                NeuralConfiguration = null;
            }               
            else
                CurrentSetupState = SetupState.CreateNew;
        }

        public void SetPause()
        {
            if (!IsConfigured)
                return;

            if (IsLearningRunning)
                IsLearningRunning = false;
            else
                IsLearningRunning = true;
        }
    }

    public class IntervalStockIdentifier
    {
        public DataInterval Interval { get; set; }
        public StockIdentifier Identifier { get; set; }

        public IntervalStockIdentifier(DataInterval interval, StockIdentifier identifier)
        {
            Interval = interval;
            Identifier = identifier;
        }
    }
}
