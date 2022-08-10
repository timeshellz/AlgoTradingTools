using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlgoTrading.Stocks
{
    public interface IStockSource
    {
        Task<List<StockIdentifier>> GetAvailableStocks();
        Task<StockData> GetStock(StockIdentifier stockName, DateTime beginning, DateTime end);
        TimeSpan GetMaxTimeSpan(DataInterval interval);
        Dictionary<string, DataInterval> GetPossibleIntervals();
    }
}
