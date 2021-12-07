using System;
using OxyPlot;
using OxyPlot.Series;
using System.Linq;
using System.Windows.Forms;
using Skender.Stock.Indicators;
using System.Collections.Generic;

namespace AlgoTrading.DQN.UserInterface
{
    public partial class MainForm : Form
    {
        public DQNMainPresenter Presenter { get; private set; }

        PlotModel mainPlot;
        PlotModel lossPlot;
        PlotModel epsilonPlot;
        PlotModel profitPlotView;
        PlotModel tradeDurationPlot;
        PlotModel tradeCountPlot;
        LineSeries avgEstimatedQSeries;
        LineSeries rewardSeries;
        LineSeries avgLossSeries;
        LineSeries epsilonSeries;
        LineSeries averageTradeProfitSeries;
        LineSeries tradeCountSeries;
        ScatterSeries avgTradeDurationSeries;
        public MainForm()
        {
            InitializeComponent();

            Presenter = new DQNMainPresenter();
            Presenter.SettingsReady += Presenter_SettingsReady;
            Presenter.StocksReady += Presenter_StocksReady;
            Presenter.ControllerThreadReady += Presenter_ControllerThreadReady;
        }

        void CreatePlots()
        {
            mainPlot = new PlotModel();
            mainPlot.Title = Presenter.DQNMeta.Name;

            lossPlot = new PlotModel();

            epsilonPlot = new PlotModel();

            profitPlotView = new PlotModel();
            tradeDurationPlot = new PlotModel();
            tradeCountPlot = new PlotModel();

            avgEstimatedQSeries = new LineSeries();
            avgEstimatedQSeries.Color = OxyColor.FromRgb(200, 50, 50);
            avgEstimatedQSeries.Title = "Avg Estimated Q";
            avgEstimatedQSeries.BrokenLineColor = OxyColors.Automatic;

            rewardSeries = new LineSeries();
            rewardSeries.Color = OxyColor.FromRgb(50, 50, 200);
            rewardSeries.Title = "Reward";
            rewardSeries.BrokenLineColor = OxyColors.Automatic;

            avgLossSeries = new LineSeries();
            avgLossSeries.Color = OxyColor.FromRgb(50, 50, 50);
            avgLossSeries.Title = "Avg Loss";
            avgLossSeries.BrokenLineColor = OxyColors.Automatic;

            epsilonSeries = new LineSeries();
            epsilonSeries.Color = OxyColor.FromRgb(200, 50, 50);
            epsilonSeries.Title = "Epsilon";
            epsilonSeries.BrokenLineColor = OxyColors.Automatic;

            averageTradeProfitSeries = new LineSeries();
            averageTradeProfitSeries.Color = OxyColor.FromRgb(200, 50, 50);
            averageTradeProfitSeries.Title = "Avg Trade Profit";
            averageTradeProfitSeries.BrokenLineColor = OxyColors.Automatic;

            tradeCountSeries = new LineSeries();
            tradeCountSeries.Color = OxyColor.FromRgb(50, 50, 50);
            tradeCountSeries.Title = "Number of Trades";
            tradeCountSeries.BrokenLineColor = OxyColors.Automatic;

            avgTradeDurationSeries = new ScatterSeries();
            avgTradeDurationSeries.Title = "Avg Trade Duration";

            UpdatePlots();

            plotUpdateTimer.Enabled = true;
            plotUpdateTimer.Start();
        }

        void UpdatePlots()
        {
            //avgEstimatedQSeries.ItemsSource = Presenter.DQNMeta.DQNStatistics.Epochs.Select(e => new DataPoint(e.EpochID, e.AverageEstimatedQ)).ToList();
            rewardSeries.ItemsSource = Presenter.DQNMeta.DQNStatistics.Epochs.Select(e => new DataPoint(e.EpochID, e.TotalIterationReward)).ToList();
            avgLossSeries.ItemsSource = Presenter.DQNMeta.DQNStatistics.Epochs.Select(e => new DataPoint(e.EpochID, e.AverageLoss)).ToList();
            epsilonSeries.ItemsSource = Presenter.DQNMeta.DQNStatistics.Epochs.Select(e => new DataPoint(e.EpochID, e.FinalEpsilon)).ToList();
            averageTradeProfitSeries.ItemsSource = Presenter.DQNMeta.DQNStatistics.Epochs.Select(e => new DataPoint(e.EpochID, e.AverageTradeProfit)).ToList();
            avgTradeDurationSeries.ItemsSource = Presenter.DQNMeta.DQNStatistics.Epochs.Select(e => new ScatterPoint(e.EpochID, e.AverageTradeDuration)).ToList();
            tradeCountSeries.ItemsSource = Presenter.DQNMeta.DQNStatistics.Epochs.Select(e => new DataPoint(e.EpochID, e.TotalTrades)).ToList();

            mainPlot.Series.Clear();
            lossPlot.Series.Clear();
            epsilonPlot.Series.Clear();
            profitPlotView.Series.Clear();
            tradeDurationPlot.Series.Clear();
            tradeCountPlot.Series.Clear();

            mainPlot.Series.Add(rewardSeries);
            //lossPlot.Series.Add(avgEstimatedQSeries);
            lossPlot.Series.Add(avgLossSeries);
            epsilonPlot.Series.Add(epsilonSeries);
            profitPlotView.Series.Add(avgTradeDurationSeries);
            tradeDurationPlot.Series.Add(averageTradeProfitSeries);
            tradeCountPlot.Series.Add(tradeCountSeries);

            mainPlotView.Model = mainPlot;
            lossPlotView.Model = lossPlot;
            epsilonPlotView.Model = epsilonPlot;
            avgProfitPlotView.Model = profitPlotView;
            avgDurationPlotView.Model = tradeDurationPlot;
            tradeCountPlotView.Model = tradeCountPlot;
        }

        private void Presenter_ControllerThreadReady(object sender, EventArgs e)
        {
            BindingSource presenterSource = new BindingSource();
            presenterSource.Add(Presenter);

            Binding pauseButtonTextBinding = new Binding("Text", presenterSource, nameof(Presenter.IsControllerRunning), true);
            pauseButtonTextBinding.Format += PauseButtonTextBinding_Format;

            Binding loadButtonEnabledBinding = new Binding("Enabled", presenterSource, nameof(Presenter.IsControllerRunning), true);
            Binding createButtonEnabledBinding = new Binding("Enabled", presenterSource, nameof(Presenter.IsControllerRunning), true);
            loadButtonEnabledBinding.Format += reverseBoolFormat;
            createButtonEnabledBinding.Format += reverseBoolFormat;

            pauseButton.Enabled = true;

            pauseButton.DataBindings.Add(pauseButtonTextBinding);
            createDQNButton.DataBindings.Add(createButtonEnabledBinding);
            loadDQNButton.DataBindings.Add(loadButtonEnabledBinding);

            CreatePlots();
        }


        private void reverseBoolFormat(object sender, ConvertEventArgs e)
        {
            e.Value = !(bool)e.Value;
        }


        private void PauseButtonTextBinding_Format(object sender, ConvertEventArgs e)
        {
            e.Value = new Func<string>(() =>
            {
                if ((bool)e.Value == true)
                    return "Pause";
                else
                    return "Resume";
            }).Invoke();
        }

        private void Presenter_StocksReady(object sender, EventArgs e)
        {
            ((ListBox)currentSettingsPanel.Controls["loadedStocksTextBox"]).DataSource = Presenter.LoadedStocksStrings;
        }

        private void Presenter_SettingsReady(object sender, EventArgs e)
        {
            BindingSource brokerSettingsSource = new BindingSource();
            brokerSettingsSource.Add(Presenter.BrokerSettings);

            BindingSource agentSettingsSource = new BindingSource();
            agentSettingsSource.Add(Presenter.AgentSettings);

            BindingSource neuralSettingsSource = new BindingSource();
            neuralSettingsSource.Add(Presenter.NeuralSettings);

            initialCapitalTextBox.DataBindings.Add("Text", brokerSettingsSource, nameof(Presenter.BrokerSettings.StartCapital));
            commissionTextBox.DataBindings.Add("Text", brokerSettingsSource, nameof(Presenter.BrokerSettings.Commission));
            hiddenLayersTextBox.DataBindings.Add("Text", neuralSettingsSource, nameof(Presenter.NeuralSettings.HiddenLayerCount));
            learningRateTextBox.DataBindings.Add("Text", neuralSettingsSource, nameof(Presenter.NeuralSettings.LearningRate));
            bufferSizeTextBox.DataBindings.Add("Text", agentSettingsSource, nameof(Presenter.AgentSettings.MemoryBuferSize));
            batchSizeTextBox.DataBindings.Add("Text", agentSettingsSource, nameof(Presenter.AgentSettings.BatchSize));
            discountTextBox.DataBindings.Add("Text", agentSettingsSource, nameof(Presenter.AgentSettings.Discount));
            epsilonTextBox.DataBindings.Add("Text", agentSettingsSource, nameof(Presenter.AgentSettings.InitialEpsilon));
            epsilonDecayTextBox.DataBindings.Add("Text", agentSettingsSource, nameof(Presenter.AgentSettings.EpsilonDecay));
            minimumEpsilonTextBox.DataBindings.Add("Text", agentSettingsSource, nameof(Presenter.AgentSettings.MinimumEpsilon));         
        }

        private void createDQNButton_Click(object sender, EventArgs e)
        {
            DQNSetupPresenter dQNSetupPresenter = new DQNSetupPresenter(Presenter);
            NeuralSetupForm setupForm = new NeuralSetupForm(dQNSetupPresenter);
            setupForm.ShowDialog();
        }

        private void loadDQNButton_Click(object sender, EventArgs e)
        {
            SelectDQNForm selectDQN = new SelectDQNForm(new DQNSelectPresenter(Presenter));
            selectDQN.ShowDialog();
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            Presenter.ControllerStartStop();
        }

        private void plotUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                UpdatePlots();
            }
            catch
            { }
            
            mainPlotView.InvalidatePlot(true);
            lossPlotView.InvalidatePlot(true);
            epsilonPlotView.InvalidatePlot(true);
            profitPlotView.InvalidatePlot(true);
            avgDurationPlotView.InvalidatePlot(true);
            tradeCountPlotView.InvalidatePlot(true);
        }
    }
}
