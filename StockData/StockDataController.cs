using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AlgoTrading.Stocks
{
    public class StockDataController
    {
        public static string BaseURL = @"https://query1.finance.yahoo.com/v8/finance/chart/";
        public Dictionary<DataInterval, List<StockData>> Stocks { get; private set; }

        List<StockDataFileManager> FileManagers;
        
        public StockDataController()
        {
            Stocks = new Dictionary<DataInterval, List<StockData>>();
            FileManagers = new List<StockDataFileManager>();
        }       

        public void LoadSavedStockData(List<string> stockFileNames)
        {
            Stocks = new Dictionary<DataInterval, List<StockData>>();

            foreach(string fileName in stockFileNames)
            {
                StockDataFileManager newManager = new StockDataFileManager(fileName);
                StockData newStock = newManager.LoadStockData();
                newStock.UpdateIndicators();

                if (!Stocks.ContainsKey(newStock.Interval))
                    Stocks.Add(newStock.Interval, new List<StockData>());
                Stocks[newStock.Interval].Add(newStock);

                if(!FileManagers.Exists(e => e.StockData == newManager.StockData))
                    FileManagers.Add(newManager);
            }
        }

        public List<string> ListSavedStockData()
        {
            return StockDataFileManager.GetAvailableStockData();
        }

        public static void DownloadStocks(List<string> tickerNames, DataInterval interval)
        {
            foreach (string tickerName in tickerNames)
            {
                Dictionary<int, StockBar> stockBars =
                    DownloadSingleStock(tickerName, GetEarliestTimeForInterval(interval), DateTime.Now, interval);

                StockData stock = new StockData(tickerName, stockBars, interval);

                StockDataFileManager newManager = new StockDataFileManager(stock);
                newManager.SaveStockData();
            }
        }

        public static void DownloadStocks(Dictionary<DataInterval, string> tickerIntervals)
        {
            foreach (KeyValuePair<DataInterval, string> stockInterval in tickerIntervals)
            {
                Dictionary<int, StockBar> stockBars =
                DownloadSingleStock(stockInterval.Value, GetEarliestTimeForInterval(stockInterval.Key), DateTime.Now, stockInterval.Key);

                StockData stock = new StockData(stockInterval.Value, stockBars, stockInterval.Key);
                StockDataFileManager newManager = new StockDataFileManager(stock);
                newManager.SaveStockData();
            }
        }

        static Dictionary<int, StockBar> DownloadSingleStock(string stockName, DateTime startDate, DateTime endDate, DataInterval interval)
        {
            string intervalString = ConvertIntervalToString(interval);

            long startTimeUnix = ((DateTimeOffset)startDate).ToUnixTimeSeconds();
            long endTimeUnix = ((DateTimeOffset)endDate).ToUnixTimeSeconds();
            string requestURL = BaseURL + $"{stockName}?symbol={stockName}&period1={startTimeUnix}&period2={endTimeUnix}&useYfid=true&" +
                $"interval={intervalString}&includePrePost=true&events=div%7Csplit%7Cearn&lang=en-US&region=US&crumb=uu9HCiSoK7A&corsDomain=finance.yahoo.com";

            HttpClient client = new HttpClient();
            Task<HttpResponseMessage> response = Task.Run(async () => await client.GetAsync(requestURL));
            response.Wait();

            string content = response.Result.Content.ReadAsStringAsync().Result;
            JToken jObject = JToken.Parse(content);
            JToken results = jObject["chart"]["result"][0];

            Dictionary<int, StockBar> output = new Dictionary<int, StockBar>();

            for (int i = 0; i < results["timestamp"].Children().ToList().Count() - 5; i++)
            {
                DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(results["timestamp"][i].Value<long>()).DateTime.ToUniversalTime();
                decimal openPrice = results["indicators"]["quote"][0]["open"][i].Value<decimal>();
                decimal closePrice = results["indicators"]["quote"][0]["close"][i].Value<decimal>();
                decimal highPrice = results["indicators"]["quote"][0]["high"][i].Value<decimal>();
                decimal lowPrice = results["indicators"]["quote"][0]["low"][i].Value<decimal>();
                decimal volume = results["indicators"]["quote"][0]["volume"][i].Value<decimal>();

                StockBar bar = new StockBar(dateTime, openPrice, closePrice, highPrice, lowPrice, volume);

                output.Add(i, bar);
            }

            return output;
        }

        public static string ConvertIntervalToString(DataInterval dataInterval)
        {
            switch (dataInterval)
            {
                case DataInterval.FiveMinute:
                    return "5m";
                case DataInterval.FifteenMinute:
                    return "15m";
                case DataInterval.HalfHour:
                    return "30m";
                case DataInterval.Hour:
                    return "1h";
                case DataInterval.FourHour:
                    return "4h";
                case DataInterval.Day:
                    return "1d";
                case DataInterval.Week:
                    return "1wk";
                case DataInterval.Month:
                    return "1mo";
                case DataInterval.ThreeMonths:
                    return "3mo";
            }

            return "1d";
        }

        public static Dictionary<string, DataInterval> GetPossibleIntervals()
        {
            Dictionary<string, DataInterval> output = new Dictionary<string, DataInterval>();

            output.Add("Five Minutes", DataInterval.FiveMinute);
            output.Add("Fifteen Minutes", DataInterval.FifteenMinute);
            output.Add("Half Hour", DataInterval.HalfHour);
            output.Add("Hour", DataInterval.Hour);
            output.Add("Four Hours", DataInterval.FourHour);
            output.Add("Day", DataInterval.Day);
            output.Add("Week", DataInterval.Week);
            output.Add("Month", DataInterval.Month);
            output.Add("Three Months", DataInterval.ThreeMonths);

            return output;
        }

        public static DateTime GetEarliestTimeForInterval(DataInterval interval)
        {
            switch (interval)
            {
                case DataInterval.FiveMinute:
                    return DateTime.Now - new TimeSpan(10, 0, 0, 0);
                case DataInterval.FifteenMinute:
                case DataInterval.HalfHour:
                    return DateTime.Now - new TimeSpan(25, 0, 0, 0);
                case DataInterval.Hour:
                case DataInterval.FourHour:
                    return DateTime.Now - new TimeSpan(175, 0, 0, 0);
            }

            return DateTime.Now - new TimeSpan(20000, 0, 0, 0);
        }
    }
}
