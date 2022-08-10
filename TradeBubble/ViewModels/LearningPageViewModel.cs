﻿using AlgoTrading.Agent.Learning;
using AlgoTrading.Broker;
using AlgoTrading.Broker.Statistics;
using AlgoTrading.DQN.Learning;
using AlgoTrading.DQN.Statistics;
using AlgoTrading.Neural;
using AlgoTrading.Stocks;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Threading.Tasks;
using TradeBubble.Services;

namespace TradeBubble.ViewModels
{
    public class LearningPageViewModel : PageViewModel
    {
        private LearningManagerService learningManager;

        private Timer refreshTimer;

        private SetupState currentSetupState = SetupState.None;
        private bool isCreatingDQN = false;
        private bool isExportingPositions = false;

        public Dictionary<DataInterval, List<StockIdentifier>> AvailableStocks { get; set; } = new Dictionary<DataInterval, List<StockIdentifier>>();
        public List<StockIdentifier> DisplayedStocks { get; set; } = new List<StockIdentifier>();
        public List<StockIdentifier> SelectedStocks { get; set; } = new List<StockIdentifier>();

        public Dictionary<int, EpochStatistics> LearningEpochs { get; set; } = new Dictionary<int, EpochStatistics>();
        public Dictionary<int, EpochStatistics> SkilledEpochs { get; set; } = new Dictionary<int, EpochStatistics>();
        public EpochStatistics BestEpoch { get; set; }

        public enum SetupState { LoadSaved, CreateNew, SelectStocks, None };

        public LearningAgentConfiguration AgentConfiguration { get; set; }
        public NeuralConfiguration NeuralConfiguration { get; set; }
        public BrokerConfiguration BrokerConfiguration { get; set; }

        public TradedStockStatistics LastBestTradedStock { get; set; }
        public TradedStockStatistics RecordBestTradedStock { get; set; }


        public bool IsConfigured { get; set; } = false || (LearningManagerService.AreSettingsReady);

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

                if (value)
                {
                    CurrentSetupState = SetupState.CreateNew;

                    AgentConfiguration = new LearningAgentConfiguration();
                    NeuralConfiguration = new NeuralConfiguration();
                    BrokerConfiguration = new BrokerConfiguration();
                }

                OnPropertyChanged();
            }
        }

        public bool IsExportingPositions
        {
            get => isExportingPositions;
            set
            {
                isExportingPositions = value;

                OnPropertyChanged();
            }
        }

        public LearningPageViewModel(LearningManagerService learningManagerService)
        {
            learningManager = learningManagerService;
            refreshTimer = new Timer(5000);
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
            //LearningManagerService.StatisticsUpdated += StatisticsUpdated;
        }

        private void StockDataFetched(object sender, StockDataFetchedEventArgs e)
        {
            if (e.Type == DataType.Saved)
            {
                if (!AvailableStocks.ContainsKey(e.Interval))
                    AvailableStocks.Add(e.Interval, new List<StockIdentifier>());

                AvailableStocks[e.Interval] = e.StockIdentifiers;

                DisplayedStocks = AvailableStocks.SelectMany(e => e.Value).ToList();
            }

            OnPropertyChanged();
        }

        private void StatisticsUpdated(object sender, LearningStatisticsUpdatedEventArgs e)
        {
            LearningEpochs = e.Statistics.LearningEpochs.ToDictionary(k => k.EpochOrder);
            SkilledEpochs = e.Statistics.SkilledEpochs.ToDictionary(k => k.EpochOrder);

            BestEpoch = e.Statistics.BestSkilledEpoch;

            if (BestEpoch != null)
                RecordBestTradedStock = BestEpoch.BrokerSessionStatistics.BestTradedStock;

            if (SkilledEpochs.Count > 0)
                LastBestTradedStock = SkilledEpochs.Last().Value.BrokerSessionStatistics.BestTradedStock;

            OnPropertyChanged();
        }

        private void ConfirmSetup()
        {
            BrokerConfiguration.StockIdentifiers = SelectedStocks.Distinct().ToList();

            LearningManagerService.DirectorConfiguration = new LearningDirectorConfiguration(AgentConfiguration, NeuralConfiguration, BrokerConfiguration);

            IsCreatingDQN = false;
            IsConfigured = true;
            LearningManagerService.AreSettingsReady = true;
        }

        public void AdvanceSetup()
        {
            if (CurrentSetupState == SetupState.CreateNew)
                CurrentSetupState = SetupState.SelectStocks;
            else if (CurrentSetupState != SetupState.None)
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
}
