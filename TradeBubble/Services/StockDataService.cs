using AlgoTrading.DQN;
using AlgoTrading.Stocks;
using AlgoTrading.Stocks.Tinkoff;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Channels;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Microsoft.EntityFrameworkCore;
using AlgoTrading.Stocks.Persistence;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace TradeBubble.Services
{
    public class StockDataService : BackgroundService
    {
        public static event EventHandler<StockDataLoadedEventArgs> StockDataLoaded;
        public static event EventHandler<StockDataFetchedEventArgs> StockDataFetched;

        private static ConcurrentQueue<Func<CancellationToken, ValueTask>> fetchQueue = new ConcurrentQueue<Func<CancellationToken, ValueTask>>();
        private static ConcurrentQueue<Func<CancellationToken, ValueTask>> downloadQueue = new ConcurrentQueue<Func<CancellationToken, ValueTask>>();

        private static TinkoffStockSource stockLoader;

        private static IStockPersistenceManager persistenceManager;

        private static StockDataService instance;

        public StockDataService(InvestApiClient client,
            IStockPersistenceManager persistence)
        {
            persistenceManager = persistence;

            stockLoader = new TinkoffStockSource(client);
            instance = this;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                List<Task> queueManageTasks = new List<Task>();

                queueManageTasks.Add(Task.Run(() => ManageQueue(fetchQueue, stoppingToken)));
                queueManageTasks.Add(Task.Run(() => ManageQueue(downloadQueue, stoppingToken)));

                await Task.WhenAll(queueManageTasks);
            }
        }

        private async Task ManageQueue(ConcurrentQueue<Func<CancellationToken, ValueTask>> queue, CancellationToken stoppingToken)
        {
            while(true)
            {
                while (queue.IsEmpty)
                    await Task.Delay(300);

                Func<CancellationToken, ValueTask> request;
                queue.TryDequeue(out request);

                await request(stoppingToken);

                if (stoppingToken.IsCancellationRequested)
                    break;
            }
        }

        public static void DownloadStocks(List<StockIdentifier> stocks, DataInterval interval, DateTime start, DateTime end)
        {
            foreach(var stockId in stocks)
            {                
                downloadQueue.Enqueue(async (token) =>
                {
                    StockData stockData = null;
                    bool success = true;

                    try
                    {
                        stockData = await stockLoader.GetStock(stockId, interval, start, end);

                        await persistenceManager.SaveStockData(stockData);
                    }
                    catch (Exception e)
                    {
                        success = false;
                    }

                    StockDataLoaded?.Invoke(instance, new StockDataLoadedEventArgs(stockData, DataType.Downloaded, success));
                });
            }            
        }

        public static void GetSavedStockData(IntervalStockIdentifier identifier)
        {
            fetchQueue.Enqueue(async (token) =>
            {
                var result = await persistenceManager.LoadStockData(identifier);
                bool success = result != null;

                StockDataLoaded?.Invoke(instance, new StockDataLoadedEventArgs(result, DataType.Saved, success));
            });
        }

        public static void FetchAvailableStocks()
        {
            fetchQueue.Enqueue(async (token) =>
            {
                var availableStocks = await stockLoader.GetAvailableStocks();

                StockDataFetched?.Invoke(instance, new StockDataFetchedEventArgs(availableStocks, DataInterval.Any, DataType.Available));
            });
        }

        public static Dictionary<string, DataInterval> GetAvailableDataIntervals()
        {
            return stockLoader.GetPossibleIntervals();
        }

        public static void FetchSavedStocks(DataInterval interval)
        {
            fetchQueue.Enqueue(async (token) =>
            {
                StockDataFetched?.Invoke(instance, 
                    new StockDataFetchedEventArgs(await persistenceManager.LoadStockIdentifiers(interval), interval, DataType.Saved));
            });
        }          

        public static void FetchSavedStocks()
        {
            fetchQueue.Enqueue(async (token) =>
            {
                var result = await persistenceManager.LoadStockIdentifiers();

                foreach(var keyValue in result)
                {
                    StockDataFetched?.Invoke(instance,
                    new StockDataFetchedEventArgs(keyValue.Value, keyValue.Key, DataType.Saved));
                }
            });
        }
    }

    public enum DataType { Available, Downloaded, Saved, Unknown}

    public abstract class StockDataServiceEventArgs : EventArgs
    {
        public DataType Type { get; }

        public bool IsSuccessful { get; }

        public StockDataServiceEventArgs(bool isSuccessful, DataType type)
        {
            IsSuccessful = isSuccessful;
            Type = type;
        }
    }

    public class StockDataLoadedEventArgs : StockDataServiceEventArgs
    {
        public StockData StockData { get; }

        public StockDataLoadedEventArgs(StockData stockData, DataType type, bool isSuccessful) : base(isSuccessful, type)
        {
            StockData = stockData;
        }
    }

    public class StockDataFetchedEventArgs : StockDataServiceEventArgs
    {
        public List<StockIdentifier> StockIdentifiers { get; }

        public DataInterval Interval { get;  }

        public StockDataFetchedEventArgs(List<StockIdentifier> stockIdentifiers, DataInterval interval, DataType type) : base(true, type)
        {
            StockIdentifiers = stockIdentifiers;
            Interval = interval;
        }
    }
}
