using Tinkoff.InvestApi.V1;

namespace AlgoTrading.Stocks.Tinkoff
{
    public static class DataIntervalExtensions
    {
        public static CandleInterval ToCandleInterval(this DataInterval interval)
        {
            switch (interval)
            {
                case DataInterval.Minute:
                    return CandleInterval._1Min;
                case DataInterval.FiveMinute:
                    return CandleInterval._5Min;
                case DataInterval.FifteenMinute:
                    return CandleInterval._15Min;
                case DataInterval.Hour:
                    return CandleInterval.Hour;
                case DataInterval.Day:
                    return CandleInterval.Day;
            }

            return CandleInterval.Unspecified;
        }
    }
}
