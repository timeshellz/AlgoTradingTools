using System;
using System.Collections.Generic;
using System.Text;
using AlgoTrading.Stocks;

namespace AlgoTrading.Stocks
{
    public interface IIndicatorProvider
    {
        List<IndicatorBar> GetIndicators(List<StockBar> bars);
    }
}
