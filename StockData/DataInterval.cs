using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoTrading.Stocks
{
    public enum DataInterval { Minute, FiveMinute, FifteenMinute, HalfHour, Hour, FourHour, Day, Week, Month, ThreeMonths, Any }

    public static class DataIntervalExtensions
    {
        public static TimeSpan GetTimeSpan(this DataInterval interval)
        {
            switch(interval)
            {
                case DataInterval.Minute:
                    return new TimeSpan(0, 1, 0);
                case DataInterval.FiveMinute:
                    return new TimeSpan(0, 5, 0);
                case DataInterval.FifteenMinute:
                    return new TimeSpan(0, 15, 0);
                case DataInterval.HalfHour:
                    return new TimeSpan(0, 30, 0);
                case DataInterval.Hour:
                    return new TimeSpan(1, 0, 0);
                case DataInterval.FourHour:
                    return new TimeSpan(4, 0, 0);
                case DataInterval.Day:
                    return new TimeSpan(1, 0, 0, 0);
                case DataInterval.Week:
                    return new TimeSpan(7, 0, 0, 0);
                case DataInterval.Month:
                    return new TimeSpan(30, 0, 0, 0, 0);
                case DataInterval.ThreeMonths:
                    return new TimeSpan(90, 0, 0, 0, 0);
            }

            return new TimeSpan(0);
        }

        public static DataInterval GetInterval(this TimeSpan span)
        {
            if (span == TimeSpan.FromMinutes(1))
                return DataInterval.Minute;
            if (span == TimeSpan.FromMinutes(5))
                return DataInterval.FiveMinute;
            if (span == TimeSpan.FromMinutes(15))
                return DataInterval.FifteenMinute;
            if (span == TimeSpan.FromMinutes(30))
                return DataInterval.HalfHour;
            if (span == TimeSpan.FromHours(1))
                return DataInterval.Hour;
            if (span == TimeSpan.FromHours(4))
                return DataInterval.Hour;
            if (span == TimeSpan.FromDays(1))
                return DataInterval.Day;
            if (span == TimeSpan.FromDays(7))
                return DataInterval.Week;
            if (span == TimeSpan.FromDays(30))
                return DataInterval.Month;
            if (span == TimeSpan.FromDays(90))
                return DataInterval.ThreeMonths;

            return DataInterval.Minute;
        }
    }
}
