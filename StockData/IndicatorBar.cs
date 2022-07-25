using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoTrading.Stocks
{
    public class IndicatorBar
    {
        public DateTime Date { get; set; }
        public decimal? Stochastic { get; set; }
        public decimal? StochasticSignal { get; set; }
        public decimal? SMA200 { get; set; }
        public decimal? SMA50 { get; set; }
        public decimal? DEMA { get; set; }
        public decimal? DEMAROC { get; set; }
        public decimal? MACD { get; set; }
        public decimal? MACDSignal { get; set; }
        public decimal? MACDHistogram { get; set; }
        public decimal? BBHigh { get; set; }
        public decimal? BBMid { get; set; }
        public decimal? BBLow { get; set; }
        public decimal? OBVROC { get; set; }
        public decimal? ADX { get; set; }
        public decimal? AroonUp { get; set; }
        public decimal? AroonDown { get; set; }
        public decimal? TripleDerivative { get; set; }
    }

    public static class IndicatorBarExtensions
    {
        public static bool IsValid(this IndicatorBar bar)
        {
            if (bar.Stochastic == null ||
                bar.StochasticSignal == null ||
                bar.SMA200 == null ||
                bar.DEMA == null ||
                bar.DEMAROC == null ||
                bar.MACD == null ||
                bar.MACDSignal == null ||
                bar.BBHigh == null ||
                bar.BBMid == null ||
                bar.BBLow == null ||
                bar.OBVROC == null ||
                bar.ADX == null ||
                bar.AroonUp == null ||
                bar.AroonDown == null ||
                bar.TripleDerivative == null)
                return false;

            return true;
        }
    }
}
