using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Channels;
using Tinkoff.InvestApi;
using AlgoTrading.Stocks;
using AlgoTrading.Stocks.Tinkoff;
using TradeBubble.Services;

namespace TradeBubble.ViewModels
{
    public class StockManagementViewModel : PageViewModel
    {
        public Dictionary<string, StockIdentifier> AllStocks { get; set; } = new Dictionary<string, StockIdentifier>();
        public Dictionary<string, StockIdentifier> AvailableStocks { get; set; } = new Dictionary<string, StockIdentifier>();      
        public Dictionary<string, StockIdentifier> SelectedStocks { get; set; } = new Dictionary<string, StockIdentifier>();
        public Dictionary<string, StockIdentifier> SavedStocks { get; set; }  = new Dictionary<string, StockIdentifier>();

        public List<DataInterval> AvailableIntervals { get; set; } = new List<DataInterval>();

        private DataInterval selectedInterval = DataInterval.Day;

        private int remainingDownloads = 0;
        private bool isDisplayingStockData = false;

        public bool IsDisplayingStockData
        {
            get => isDisplayingStockData;
            set
            {
                isDisplayingStockData = value;

                OnPropertyChanged();
            }
        }

        public StockData DisplayedStockData { get; set; }

        public DataInterval SelectedInterval
        {
            get => selectedInterval;
            set
            {
                selectedInterval = value;

                StockDataService.FetchSavedStocks(SelectedInterval);

                OnPropertyChanged();
            }
        }
        
        public int RemainingDownloads 
        { 
            get => remainingDownloads; 
            private set
            {
                remainingDownloads = value;

                OnPropertyChanged();
            }
        }

        public StockManagementViewModel() { }

        protected override async Task LoadData()
        {
            AvailableIntervals = StockDataService.GetAvailableDataIntervals().Values.ToList();

            StockDataService.StockDataLoaded += StockDataLoaded;
            StockDataService.StockDataFetched += StockDataFetched;

            StockDataService.FetchAvailableStocks();
            StockDataService.FetchSavedStocks(SelectedInterval);                  
        }

        private void StockDataLoaded(object sender, StockDataLoadedEventArgs e)
        {
            if (e.Type == DataType.Downloaded)
            {
                RemainingDownloads--;
                StockDataService.FetchSavedStocks(SelectedInterval);
            }

            if(e.Type == DataType.Saved && e.IsSuccessful && e.StockData.Interval == SelectedInterval)
                DisplayedStockData = e.StockData;
            else if (!e.IsSuccessful)
                IsDisplayingStockData = false;

            OnPropertyChanged();
        }

        private void StockDataFetched(object sender, StockDataFetchedEventArgs e)
        {
            if (e.Type == DataType.Available)
                AllStocks = e.StockIdentifiers.ToDictionary(k => k.FIGI);

            if (e.Type == DataType.Saved && e.Interval == SelectedInterval)
            {
                SavedStocks = e.StockIdentifiers.ToDictionary(k => k.FIGI);
                UpdateAvailableStocks();
            }

            OnPropertyChanged();
        }

        private void UpdateAvailableStocks()
        {
            AvailableStocks = AllStocks.Values.Where(e => !SavedStocks.ContainsKey(e.FIGI)).ToDictionary(k => k.FIGI);
        }

        public void DownloadSelectedStocks()
        {
            RemainingDownloads += SelectedStocks.Count;
            StockDataService.DownloadStocks(SelectedStocks.Values.ToList(), SelectedInterval, DateTime.Now, DateTime.Now);

            DeselectAllStocks();
        }

        public void SelectAllStocks()
        {
            foreach(var stock in AvailableStocks)
            {
                SelectStock(stock.Value);
            }
        }

        public void DeselectAllStocks()
        {
            foreach (var stock in SelectedStocks.Values)
                DeselectStock(stock);

            SelectedStocks = new Dictionary<string, StockIdentifier>();
        }

        public void SelectStock(StockIdentifier stock)
        {
            if (!SelectedStocks.ContainsKey(stock.FIGI) && !SavedStocks.ContainsKey(stock.FIGI))
                SelectedStocks.Add(stock.FIGI, stock);

            AvailableStocks.Remove(stock.FIGI);
        }

        public void DeselectStock(StockIdentifier stock)
        {
            if (!AvailableStocks.ContainsKey(stock.FIGI))
                AvailableStocks.Add(stock.FIGI, stock);

            SelectedStocks.Remove(stock.FIGI);
        }

        public void DisplayStock(StockIdentifier stock)
        {
            IsDisplayingStockData = true;
            StockDataService.GetSavedStockData(new IntervalStockIdentifier(SelectedInterval, stock));
        }

        public void CloseDisplayedStock()
        {
            IsDisplayingStockData = false;
            DisplayedStockData = null;
        }
    }
}
