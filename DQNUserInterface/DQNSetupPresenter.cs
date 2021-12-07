using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrading.DQN;
using AlgoTrading.Stocks;
using AlgoTrading.Neural;
using AlgoTrading.SimulatedBroker;

namespace AlgoTrading.DQN.UserInterface
{
    public class DQNSetupPresenter
    {
        public DQNMainPresenter MainPresenter { get; private set; }

        public string InitialCapital { get; set; }
        public string Commission { get; set; }
        public string NetworkName { get; set; }
        public string HiddenLayers { get; set; }
        public string LearningRate { get; set; }
        public string AgentName { get; set; }
        public string BufferSize { get; set; }
        public string BatchSize { get; set; }
        public string StepCount { get; set; }
        public string Discount { get; set; }
        public string Epsilon { get; set; }
        public string EpsilonDecay { get; set; }
        public string MinimumEpsilon { get; set; }
        public string Tau { get; set; }

        public List<string> AvailableStocks { get; set; }
        public Dictionary<string, DataInterval> Intervals { get; private set; }

        public List<int> SelectedIntervals { get; set; }
        public List<int> SelectedAvailableStocks { get; set; }
        public List<int> SelectedAddedStocks { get; set; }
        public string SelectedDownloadStock { get; set; }
        
        public List<string> AddedStocks { get; private set; }

        public StockDataController StockDataController { get; private set; }

        public DQNSetupPresenter(DQNMainPresenter mainPresenter)
        {
            MainPresenter = mainPresenter;

            InitialCapital = "600";
            Commission = "0.0003";
            NetworkName = "New Network";
            HiddenLayers = "3";
            LearningRate = "0.01";
            AgentName = "New Agent";
            BufferSize = "500000";
            BatchSize = "32";
            StepCount = "10";
            Discount = "0.999";
            Epsilon = "0.99";
            EpsilonDecay = "0.99";
            MinimumEpsilon = "0.1";
            Tau = "0.01";

            SelectedDownloadStock = "";

            Intervals = StockDataController.GetPossibleIntervals();
            StockDataController = new StockDataController();
            AddedStocks = new List<string>();
            AvailableStocks = new List<string>();
        }

        public void GenerateDQN()
        {
            NeuralSettings neuralSettings = new NeuralSettings(NetworkName, BrokerEmulator.GetAllPossibleOutputs(), BrokerEmulator.GetAllPossibleActions(),
                                            Convert.ToInt32(HiddenLayers), Convert.ToDouble(LearningRate));//, NeuralSettings.PredictionMechanism.CategoricalCrossEntropy, 51);
            AgentSettings agentSettings = new AgentSettings(AgentName, Convert.ToInt32(BufferSize), Convert.ToInt32(BatchSize), Convert.ToDouble(Discount),
                                            Convert.ToDouble(Epsilon), Convert.ToDouble(MinimumEpsilon), Convert.ToDouble(EpsilonDecay), Convert.ToInt32(StepCount),
                                            Convert.ToDouble(Tau));
                                            
            BrokerSettings brokerSettings = new BrokerSettings(AddedStocks, Convert.ToDecimal(InitialCapital), Convert.ToDecimal(Commission));

            MainPresenter.SetupNewDQN(brokerSettings, neuralSettings, agentSettings);
        }

        public void ConfirmStockDataSelection()
        {
            MainPresenter.SetStockData(AddedStocks);           
        }

        public void GetAvailableStocks()
        {          
            AvailableStocks = StockDataController.ListSavedStockData().Where(e => !AddedStocks.Contains(e)).ToList();
        }

        public void DownloadStock()
        {
            Dictionary<DataInterval, string> stockIntervals = new Dictionary<DataInterval, string>();

            foreach(int intervalIndex in SelectedIntervals)
            {
                stockIntervals.Add(Intervals.ElementAt(intervalIndex).Value, SelectedDownloadStock);
            }

            StockDataController.DownloadStocks(stockIntervals);
        }

        public void MoveStockToLoaded()
        {
            if (SelectedAvailableStocks.Count == 0)
                return;

            foreach(int index in SelectedAvailableStocks)
            {
                AddedStocks.Add(AvailableStocks.ElementAt(index));
                AvailableStocks.RemoveAt(index);
            }
        }

        public void MoveAllStocksToLoaded()
        {
            AddedStocks.AddRange(AvailableStocks);
            AvailableStocks = new List<string>();
        }

        public void MoveStockFromLoaded()
        {
            if (SelectedAddedStocks.Count == 0)
                return;

            foreach (int index in SelectedAddedStocks)
            {
                AvailableStocks.Add(AddedStocks.ElementAt(index));
                AddedStocks.RemoveAt(index);
            }
        }

        public void MoveAllStocksFromLoaded()
        {
            AvailableStocks.AddRange(AddedStocks);
            AddedStocks = new List<string>();
        }
    }
}
