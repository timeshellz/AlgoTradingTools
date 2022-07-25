using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AlgoTrading.Stocks.Yahoo
{
    class YahooStockSource : IStockSource
    {
        private static string yahooURL = @"https://query1.finance.yahoo.com/v8/finance/chart/";
       
        public Task<List<StockIdentifier>> GetAvailableStocks()
        {
            throw new NotImplementedException();
        }

        public async Task<StockData> GetStock(StockIdentifier stockName, DataInterval interval, DateTime beginning, DateTime end)
        {
            string intervalString = interval.ToIntervalString();

            long startTimeUnix = ((DateTimeOffset)beginning).ToUnixTimeSeconds();
            long endTimeUnix = ((DateTimeOffset)end).ToUnixTimeSeconds();
            string requestURL = yahooURL + $"{stockName}?symbol={stockName}&period1={startTimeUnix}&period2={endTimeUnix}&useYfid=true&" +
                $"interval={intervalString}&includePrePost=true&events=div%7Csplit%7Cearn&lang=en-US&region=US&crumb=uu9HCiSoK7A&corsDomain=finance.yahoo.com";

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(requestURL);

            string content = response.Content.ReadAsStringAsync().Result;
            JToken jObject = JToken.Parse(content);
            JToken results = jObject["chart"]["result"][0];

            List<StockBar> bars = new List<StockBar>();

            for (int i = 0; i < results["timestamp"].Children().ToList().Count() - 5; i++)
            {
                DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(results["timestamp"][i].Value<long>()).DateTime.ToUniversalTime();
                decimal openPrice = results["indicators"]["quote"][0]["open"][i].Value<decimal>();
                decimal closePrice = results["indicators"]["quote"][0]["close"][i].Value<decimal>();
                decimal highPrice = results["indicators"]["quote"][0]["high"][i].Value<decimal>();
                decimal lowPrice = results["indicators"]["quote"][0]["low"][i].Value<decimal>();
                decimal volume = results["indicators"]["quote"][0]["volume"][i].Value<decimal>();

                StockBar bar = new StockBar(dateTime, openPrice, closePrice, highPrice, lowPrice, volume);

                bars.Add(bar);
            }

            StockData result = new StockData(stockName, bars, interval);

            return result;
        }

        public TimeSpan GetMaxTimeSpan(DataInterval interval)
        {
            switch (interval)
            {
                case DataInterval.FiveMinute:
                    return new TimeSpan(10, 0, 0, 0);
                case DataInterval.FifteenMinute:
                case DataInterval.HalfHour:
                    return new TimeSpan(25, 0, 0, 0);
                case DataInterval.Hour:
                case DataInterval.FourHour:
                    return new TimeSpan(175, 0, 0, 0);
            }

            return new TimeSpan(20000, 0, 0, 0);
        }

        public Dictionary<string, DataInterval> GetPossibleIntervals()
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
    }
}
