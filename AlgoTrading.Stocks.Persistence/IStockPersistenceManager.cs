using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlgoTrading.Stocks.Persistence
{
    public interface IStockPersistenceManager
    {
        Task<List<StockData>> LoadStockData(List<StockIdentifier> stockInfo);
        Task<List<StockData>> LoadStockData(DataInterval interval);
        Task<StockData> LoadStockData(StockIdentifier stockName);
        Task<List<StockIdentifier>> LoadStockIdentifiers(DataInterval interval);
        Task<Dictionary<DataInterval, List<StockIdentifier>>> LoadStockIdentifiers();
        Task SaveStockData(StockData stockData);
        Task SaveStockData(List<StockData> stockDatas);
    }
}
