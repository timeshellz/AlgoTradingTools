using System.Collections.Generic;

namespace AlgoTrading.Stocks
{
    public interface IIndicatorProvider
    {
        List<IndicatorBar> GetIndicators(List<StockBar> bars);
    }
}
