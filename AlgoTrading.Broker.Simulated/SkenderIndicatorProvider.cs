using AlgoTrading.Stocks;
using System;
using System.Collections.Generic;
using System.Text;
using Skender.Stock.Indicators;
using System.Linq;

namespace AlgoTrading.Broker.Simulated
{
    public class SkenderIndicatorProvider : IIndicatorProvider
    {
        public List<IndicatorBar> GetIndicators(List<StockBar> bars)
        {
            var stockBars = bars;
            var indicatorBars = new List<IndicatorBar>(stockBars.Select(e => new IndicatorBar() { Date = e.Date })).ToList();
            
            var stochResults = stockBars.GetStoch(5, 1, 3).ToDictionary(k => k.Date);
            var rsiResults = stockBars.GetRsi().ToDictionary(k => k.Date);
            var sma200Results = stockBars.GetSma(200).ToDictionary(k => k.Date);           
            var demaResults = stockBars.GetDoubleEma(20);
            var demaRocResults = ConvertToQuotes(demaResults).GetRoc(4).ToDictionary(k => k.Date);
            var demaResultsDictionary = demaResults.ToDictionary(k => k.Date);
            var rocResults = stockBars.GetRoc(4).ToDictionary(k => k.Date);
            var longRocResults = stockBars.GetRoc(150).ToDictionary(k => k.Date);
            var macdResults = stockBars.GetMacd(5, 15, 2).ToDictionary(k => k.Date);
            var bbResults = stockBars.GetBollingerBands().ToDictionary(k => k.Date);
            var bopResults = stockBars.GetBop().ToDictionary(k => k.Date);
            var obvResults = stockBars.GetObv();
            var obvRocResults = ConvertToQuotes(obvResults).GetRoc(20).ToDictionary(k => k.Date);
            var obvResultsDictionary = obvResults.ToDictionary(k => k.Date);
            var adxResults = stockBars.GetAdx(14).ToDictionary(k => k.Date);
            var aroonResults = stockBars.GetAroon(14).ToDictionary(k => k.Date);

            var sma50Results = stockBars.GetSma(50);
            var sma50Roc100Results = ConvertToQuotes(sma50Results).GetRoc(100);
            var sma50ResultsDictionary = sma50Results.ToDictionary(k => k.Date);

            var roc100Roc100Results = ConvertToQuotes(sma50Roc100Results).GetRoc(100);
            var roc100Roc10Results = ConvertToQuotes(roc100Roc100Results).GetRoc(10)
                .Where(x => x.Roc != null)
                .Select(e => 
                {
                    e.Roc /= 100; 
                    return e; 
                }).ToDictionary(k => k.Date);

            indicatorBars.ForEach(e =>
            {
                e.Stochastic = stochResults[e.Date].Oscillator;
                e.StochasticSignal = stochResults[e.Date].Signal;
                e.RSI = rsiResults[e.Date].Rsi;
                e.SMA200 = sma200Results[e.Date].Sma;
                e.SMA50 = sma50ResultsDictionary[e.Date].Sma;
                e.DEMA = demaResultsDictionary[e.Date].Dema;                
                e.MACD = macdResults[e.Date].Macd;
                e.MACDSignal = macdResults[e.Date].Signal;
                e.MACDHistogram = macdResults[e.Date].Histogram;
                e.BBHigh = bbResults[e.Date].UpperBand;
                e.BBMid = bbResults[e.Date].Sma;
                e.BBLow = bbResults[e.Date].LowerBand;                
                e.ADX = adxResults[e.Date].Adx;
                e.AroonUp = aroonResults[e.Date].AroonUp;
                e.AroonDown = aroonResults[e.Date].AroonDown;
                e.ROC = rocResults[e.Date].Roc;

                if (demaRocResults.ContainsKey(e.Date))
                    e.DEMAROC = demaRocResults[e.Date].Roc;
                else
                    e.DEMAROC = null;

                if (obvRocResults.ContainsKey(e.Date))
                    e.OBVROC = obvRocResults[e.Date].Roc;
                else
                    e.OBVROC = null;

                if (roc100Roc10Results.ContainsKey(e.Date))
                    e.TripleDerivative = roc100Roc10Results[e.Date].Roc;
                else
                    e.TripleDerivative = null;
            }
            );                              

            return indicatorBars;
        }

        private List<Quote> ConvertToQuotes(IEnumerable<DemaResult> demaResults)
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

        private List<Quote> ConvertToQuotes(IEnumerable<RocResult> rocResults)
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

        private List<Quote> ConvertToQuotes(IEnumerable<ObvResult> obvResults)
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

        private List<Quote> ConvertToQuotes(IEnumerable<SmaResult> smaResults)
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
