using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;

using Skender.Stock.Indicators;

namespace AlgoTrading.Stocks
{
    
    public class StockData
    {             
        public StockIdentifier Identifier { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }       
        public DataInterval Interval { get; private set; }
        public SortedList<DateTime, StockBar> Bars { get; private set; }
        public SortedList<DateTime, IndicatorBar> Indicators { get; private set; }

        private StockData() { }

        public static StockDataBuilder GetBuilder()
        {
            return new StockDataBuilder(new StockData());
        }

        public void SetIndicators(IIndicatorProvider provider)
        {
            if (Bars != null && Bars.Count > 0)
            {
                var indicators = provider.GetIndicators(Bars.Values.ToList()).ToDictionary(k => k.Date);

                if (indicators.All(b => Bars.ContainsKey(b.Key)))
                {
                    Indicators = new SortedList<DateTime, IndicatorBar>(indicators);

                    TrimInvalidBars();
                    return;
                }
                else
                    throw new ArgumentException("Provided indicators don't fully match stock bars.");
            }
        }

        private void TrimInvalidBars()
        {
            if (Indicators != null && Indicators.Count > 0)
            {
                var firstValidDate = Indicators.First(e => e.Value.IsValid()).Key;
                var invalidDates = new List<DateTime>(Indicators.Where(e => e.Key < firstValidDate).Select(v => v.Key));

                foreach (var invalidDate in invalidDates)
                {
                    Indicators.Remove(invalidDate);
                    Bars.Remove(invalidDate);
                }

                StartDate = Bars.First().Key;
                EndDate = Bars.Last().Key;
            }
        }

        public class StockDataBuilder
        {
            private StockData stockData;

            public StockDataBuilder(StockData stockData)
            {
                this.stockData = stockData;
                this.stockData.Interval = DataInterval.Any;
            }

            public StockData Build()
            {
                if (stockData.Identifier == null)
                    throw new ArgumentNullException("Identifier not set.");

                if(stockData.Interval == DataInterval.Any)
                    throw new ArgumentNullException("Interval not set.");

                if(stockData.Bars == null)
                    throw new ArgumentNullException("Bars not set.");

                return stockData;
            }           

            public StockDataBuilder SetIdentifier(StockIdentifier identifier)
            {
                stockData.Identifier = identifier;

                return this;
            }

            public StockDataBuilder SetInterval(DataInterval interval)
            {
                stockData.Interval = interval;

                return this;
            }

            public StockDataBuilder SetBars(List<StockBar> bars)
            {
                SetBars(new SortedList<DateTime, StockBar>(bars.ToDictionary(k => k.Date)));

                return this;
            }

            public StockDataBuilder SetBars(SortedList<DateTime, StockBar> bars)
            {
                if (bars != null && bars.Count > 0)
                {
                    stockData.Bars = bars;
                    stockData.StartDate = stockData.Bars.First().Key;
                    stockData.EndDate = stockData.Bars.Last().Key;
                }
                else
                    stockData.Bars = new SortedList<DateTime, StockBar>();

                return this;
            }

            public StockDataBuilder SetIndicators(IIndicatorProvider provider)
            {
                stockData.SetIndicators(provider);

                return this;
            }
        }
    }
}
