using System;
using System.Collections.Generic;
using System.Text;
using AlgoTrading.Stocks;
using System.Threading.Tasks;

namespace AlgoTrading.Stocks.Persistence
{
    public interface IStockPersistenceManager
    {
        Task<List<StockData>> LoadStockData(Dictionary<StockIdentifier, DataInterval> stockInfo);
        Task<List<StockData>> LoadStockData(DataInterval interval);
        Task<StockData> LoadStockData(StockIdentifier stockName, DataInterval interval);
        Task<List<StockIdentifier>> LoadStockIdentifiers(DataInterval interval);
        Task<Dictionary<DataInterval, List<StockIdentifier>>> LoadStockIdentifiers();
        Task SaveStockData(StockData stockData);
        Task SaveStockData(List<StockData> stockDatas);
    }
}
