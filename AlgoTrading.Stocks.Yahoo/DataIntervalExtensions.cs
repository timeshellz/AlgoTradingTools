using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoTrading.Stocks.Yahoo
{
    public static class DataIntervalExtensions
    {
        public static string ToIntervalString(this DataInterval interval)
        {
            switch (interval)
            {
                case DataInterval.Minute:
                    return "1m";
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
    }
}
