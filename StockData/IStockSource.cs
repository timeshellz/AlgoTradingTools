using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTrading.Stocks
{
    public interface IStockSource
    {
        Task<List<StockIdentifier>> GetAvailableStocks();      
        Task<StockData> GetStock(StockIdentifier stockName, DataInterval interval, DateTime beginning, DateTime end);
        TimeSpan GetMaxTimeSpan(DataInterval interval);
        Dictionary<string, DataInterval> GetPossibleIntervals();
    }
}
