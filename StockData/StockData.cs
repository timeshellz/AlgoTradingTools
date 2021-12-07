using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;

using Skender.Stock.Indicators;

namespace AlgoTrading.Stocks
{
    public enum DataInterval { FiveMinute, FifteenMinute, HalfHour, Hour, FourHour, Day, Week, Month, ThreeMonths }

    public class StockData
    {             
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }       
        public DataInterval Interval { get; set; }
        public Dictionary<int, StockBar> Bars { get; set; }
        [JsonIgnore]
        public Dictionary<string, IEnumerable<IResult>> Indicators { get; private set; }
        public int TotalBars { get; set; }

        string intervalString;

        public StockData(string name, Dictionary<int, StockBar> bars, DataInterval interval)
        {
            Name = name;                                
            Bars = bars;
            Interval = interval;
            intervalString = StockDataController.ConvertIntervalToString(interval);
            UpdateIndicators();
            StartDate = Bars.First().Value.Date;
            EndDate = Bars.Last().Value.Date;
            TotalBars = Bars.Count;
        }


        
        public void UpdateIndicators()
        {
            Indicators = GetIndicators();
        }

        Dictionary<string, IEnumerable<IResult>> GetIndicators()
        {
            Dictionary<string, IEnumerable<IResult>> indicators = new Dictionary<string, IEnumerable<IResult>>();

            IEnumerable<StochResult> stochResults = Bars.Values.GetStoch(5, 1, 3).Where(e => e.Oscillator != null && e.Signal != null);
            IEnumerable<SmaResult> sma200Results = Bars.Values.GetSma(200).Where(e => e.Sma != null);
            IEnumerable<SmaResult> sma50Results = Bars.Values.GetSma(50).Where(e => e.Sma != null);
            IEnumerable<DemaResult> demaResults = Bars.Values.GetDoubleEma(20).Where(e => e.Dema != null);
            IEnumerable<RocResult> demaRocResults = ConvertToQuotes(demaResults).GetRoc(4).Where(e => e.Roc != null);
            IEnumerable<RocResult> rocResults = Bars.Values.GetRoc(4).Where(e => e.Roc != null);
            IEnumerable<RocResult> longRocResults = Bars.Values.GetRoc(150).Where(e => e.Roc != null);
            IEnumerable<MacdResult> macdResults = Bars.Values.GetMacd(5, 15, 2).Where(e => e.Macd != null && e.Signal != null && e.Histogram != null);
            IEnumerable<BollingerBandsResult> bbResults = Bars.Values.GetBollingerBands().Where(e => e.LowerBand != null && e.UpperBand != null && e.Sma != null);
            IEnumerable<BopResult> bopResults = Bars.Values.GetBop().Where(e => e.Bop != null);
            IEnumerable<ObvResult> obvResults = Bars.Values.GetObv().Where(e => e.Obv != null);
            IEnumerable<RocResult> obvRocResults = ConvertToQuotes(obvResults).GetRoc(20).Where(e => e.Roc != null);
            IEnumerable<AdxResult> adxResults = Bars.Values.GetAdx(14).Where(e => e.Adx != null);
            IEnumerable<AroonResult> aroonResults = Bars.Values.GetAroon(14).Where(e => e.AroonDown != null && e.AroonUp != null);

            IEnumerable<RocResult> sma50Roc100Results = ConvertToQuotes(sma50Results).GetRoc(100);
            IEnumerable<RocResult> roc100Roc100Results = ConvertToQuotes(sma50Roc100Results).GetRoc(100);
            IEnumerable<RocResult> roc100Roc10Results = ConvertToQuotes(roc100Roc100Results).GetRoc(10).Where(e => e.Roc != null);

            for(int i = 0; i < roc100Roc10Results.Count(); i++)
            {
                roc100Roc10Results.ElementAt(i).Roc /= 100;
            }

            indicators.Add("Stochastic", stochResults);
            indicators.Add("Sma200", sma200Results);
            indicators.Add("Sma50", sma50Results);
            indicators.Add("Dema", demaResults);
            indicators.Add("DemaROC", demaRocResults);
            indicators.Add("ROC", rocResults);
            indicators.Add("LongROC", longRocResults);
            indicators.Add("MACD", macdResults);
            indicators.Add("BB", bbResults);
            indicators.Add("OBV", obvResults);
            indicators.Add("OBVROC", obvRocResults);
            indicators.Add("ADX", adxResults);
            indicators.Add("Aroon", aroonResults);
            indicators.Add("DDD", roc100Roc10Results);

            return indicators;
        }

        List<Quote> ConvertToQuotes(IEnumerable<DemaResult> demaResults)
        {
            List<Quote> demaQuotes = demaResults
                .Where(x => x.Dema != null)
                .Select(x => new Quote
                {
                    Date = x.Date,
                    Close = (decimal)x.Dema
                })
                .ToList();

            return demaQuotes;
        }

        List<Quote> ConvertToQuotes(IEnumerable<RocResult> rocResults)
        {
            List<Quote> rocQuotes = rocResults
                .Where(x => x.Roc != null)
                .Select(x => new Quote
                {
                    Date = x.Date,
                    Close = (decimal)x.Roc
                })
                .ToList();

            return rocQuotes;
        }

        List<Quote> ConvertToQuotes(IEnumerable<ObvResult> obvResults)
        {
            List<Quote> obvQuotes = obvResults
                .Where(x => x.Obv != null)
                .Select(x => new Quote
                {
                    Date = x.Date,
                    Close = (decimal)x.Obv
                })
                .ToList();

            return obvQuotes;
        }

        List<Quote> ConvertToQuotes(IEnumerable<SmaResult> smaResults)
        {
            List<Quote> smaQuotes = smaResults
                .Where(x => x.Sma != null)
                .Select(x => new Quote
                {
                    Date = x.Date,
                    Close = (decimal)x.Sma
                })
                .ToList();

            return smaQuotes;
        }        
    }
}
