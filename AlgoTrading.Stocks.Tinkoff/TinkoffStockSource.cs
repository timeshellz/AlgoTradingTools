using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

namespace AlgoTrading.Stocks.Tinkoff
{
    public class TinkoffStockSource : IStockSource
    {
        InvestApiClient client;

        private int marketDataRequestCount = 0;
        private Timer minuteTimer;

        public TinkoffStockSource(InvestApiClient client)
        {
            this.client = client;
            minuteTimer = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);

            minuteTimer.Elapsed += (o, e) => marketDataRequestCount = 0;
            minuteTimer.Start();
        }

        public async Task<StockData> GetStock(StockIdentifier stockName, DateTime beginning, DateTime end)
        {
            var share = await client.Instruments
                    .ShareByAsync(new InstrumentRequest() { Id = stockName.FIGI, IdType = InstrumentIdType.Figi });

            int limit = (await client.Users.GetUserTariffAsync()).UnaryLimits.First(e => e.Methods.Any(m => m.Contains("GetCandles"))).LimitPerMinute;

            var stockDataBuilder = StockData.GetBuilder().SetIdentifier(stockName);
            var downloadedBars = new SortedList<DateTime, StockBar>();

            DateTimeOffset spanEnd = DateTime.Now.ToUniversalTime();

            bool isDataOver = false;

            while (!isDataOver)
            {
                DateTimeOffset spanStart = spanEnd - GetMaxTimeSpan(stockName.Interval);

                var shareBars = await client.MarketData
                .GetCandlesAsync(new GetCandlesRequest()
                {
                    Figi = stockName.FIGI,
                    From = new Timestamp() { Seconds = spanStart.ToUnixTimeSeconds(), Nanos = 0 },
                    To = new Timestamp() { Seconds = spanEnd.ToUnixTimeSeconds(), Nanos = 0 },
                    Interval = stockName.Interval.ToCandleInterval()
                });

                marketDataRequestCount++;

                spanEnd = spanStart;

                if (!shareBars.Candles.All(e => e != null && e.Time != null) || shareBars.Candles.Count == 0)
                    isDataOver = true;
                else
                {
                    foreach (var stockBar in shareBars.Candles.Select(c => new StockBar(c.Time.ToDateTime(), c.Open, c.Close, c.High, c.Low, c.Volume)))
                    {
                        if (!downloadedBars.ContainsKey(stockBar.Date))
                            downloadedBars.Add(stockBar.Date, stockBar);
                    }
                }               

                while (marketDataRequestCount >= limit)
                    await Task.Delay(500);
            }

            stockDataBuilder.SetBars(downloadedBars);
            return stockDataBuilder.Build();
        }

        public async Task<List<StockIdentifier>> GetAvailableStocks()
        {
            List<StockIdentifier> result = new List<StockIdentifier>();
            var shares = await client.Instruments.SharesAsync(new InstrumentsRequest() { InstrumentStatus = InstrumentStatus.Base });

            foreach (var share in shares.Instruments)
            {
                result.Add(new StockIdentifier(share.Name, share.Figi, share.Currency, share.Sector, share.CountryOfRisk, DataInterval.Any));
            }

            return result;
        }

        public TimeSpan GetMaxTimeSpan(DataInterval interval)
        {
            switch (interval)
            {
                case DataInterval.Minute:
                case DataInterval.FiveMinute:
                case DataInterval.FifteenMinute:
                    return new TimeSpan(0, 23, 59, 59);
                case DataInterval.Hour:
                    return new TimeSpan(7, 0, 0, 0);
                case DataInterval.Day:
                    return new TimeSpan(360, 0, 0, 0);
            }

            return new TimeSpan(0);
        }

        public Dictionary<string, DataInterval> GetPossibleIntervals()
        {
            Dictionary<string, DataInterval> output = new Dictionary<string, DataInterval>();

            output.Add("One Minute", DataInterval.Minute);
            output.Add("Five Minutes", DataInterval.FiveMinute);
            output.Add("Fifteen Minutes", DataInterval.FifteenMinute);
            output.Add("Hour", DataInterval.Hour);
            output.Add("Day", DataInterval.Day);

            return output;
        }
    }
}
