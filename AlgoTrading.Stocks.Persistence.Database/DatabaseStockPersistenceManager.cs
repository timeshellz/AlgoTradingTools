using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AlgoTrading.Stocks.Persistence.Database
{
    public class DatabaseStockPersistenceManager : IStockPersistenceManager
    {
        IDbContextFactory<StockDataContext> dbFactory;

        public DatabaseStockPersistenceManager(IDbContextFactory<StockDataContext> factory)
        {
            dbFactory = factory;
        }

        public async Task<StockData> LoadStockData(IntervalStockIdentifier stockInfo)
        {
            try
            {
                using (var db = dbFactory.CreateDbContext())
                {
                    StockDataDTO result = await db.Stocks.Include(s => s.Info).Include(s => s.Bars)
                        .FirstOrDefaultAsync(e => e.FIGI == stockInfo.Identifier.FIGI && e.Interval == stockInfo.Interval.GetTimeSpan());

                    return result.GetModel();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<StockData>> LoadStockData(List<IntervalStockIdentifier> stockInfo)
        {
            try
            {
                using (var db = dbFactory.CreateDbContext())
                {
                    var figiStocks = stockInfo.ToDictionary(k => k.Identifier.FIGI);

                    List<StockData> result = await db.Stocks
                        .Where(e => figiStocks.ContainsKey(e.FIGI) && figiStocks[e.FIGI].Interval == e.Interval.GetInterval())
                        .Select(e => e.GetModel())
                        .ToListAsync();

                    return result;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<StockData>> LoadStockData(DataInterval interval)
        {
            try
            {
                using (var db = dbFactory.CreateDbContext())
                {
                    List<StockData> result = await db.Stocks.Select(s => s.GetModel()).Where(m => m.Interval == interval).ToListAsync();

                    return result;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<StockIdentifier>> LoadStockIdentifiers(DataInterval interval)
        {
            try
            {
                using (var db = dbFactory.CreateDbContext())
                {
                    List<StockIdentifier> result = await db.Stocks.Where(s => s.Interval == interval.GetTimeSpan())
                        .Select(s => s.Info.GetModel()).ToListAsync();

                    return result;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<Dictionary<DataInterval, List<StockIdentifier>>> LoadStockIdentifiers()
        {
            try
            {
                using (var db = dbFactory.CreateDbContext())
                {
                    var stocks = await db.Stocks.Include(s => s.Info).Select(s => s.GetModel()).ToListAsync();

                    Dictionary<DataInterval, List<StockIdentifier>> result = new Dictionary<DataInterval, List<StockIdentifier>>();
                    foreach (var stock in stocks)
                    {
                        if (!result.ContainsKey(stock.Interval))
                            result.Add(stock.Interval, new List<StockIdentifier>());

                        result[stock.Interval].Add(stock.Identifier);
                    }

                    return result;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task SaveStockData(StockData stockData)
        {
            try
            {
                using (var db = dbFactory.CreateDbContext())
                {
                    StockDataDTO existingData = await db.Stocks
                        .FirstOrDefaultAsync(e => e.FIGI == stockData.Identifier.FIGI && e.Interval == stockData.Interval.GetTimeSpan());

                    if (existingData != null)
                    {
                        db.Entry(existingData).CurrentValues.SetValues(stockData.GetDTO());
                        db.Stocks.Update(existingData);
                    }                        
                    else
                        db.Stocks.Add(stockData.GetDTO());

                    await db.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task SaveStockData(List<StockData> stockDatas)
        {
            throw new NotImplementedException();

            //try
            //{
            //    using (var db = dbFactory.CreateDbContext())
            //    {
            //        db.Stocks.UpdateRange(stockDatas.Select(s => s.GetDTO()));
            //        await db.SaveChangesAsync();
            //    }
            //}
            //catch (Exception e)
            //{
            //    throw;
            //}
        }
    }
}
