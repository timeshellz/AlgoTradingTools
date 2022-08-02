using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlgoTrading.Stocks;

namespace AlgoTrading.Stocks.Persistence.Database
{
    public static class StockModelExtensions
    {
        public static StockBarDTO GetDTO(this StockBar stockBar)
        {
            return new StockBarDTO()
            {
                Date = stockBar.Date,
                Open = stockBar.Open,
                Close = stockBar.Close,
                High = stockBar.High,
                Low = stockBar.Low,
                Volume = stockBar.Volume,
            };
        }

        public static StockBar GetModel(this StockBarDTO stockBarDTO)
        {
            return new StockBar(stockBarDTO.Date, 
                stockBarDTO.Open, 
                stockBarDTO.Close, 
                stockBarDTO.High, 
                stockBarDTO.Low, 
                stockBarDTO.Volume);
        }

        public static StockDataDTO GetDTO(this StockData stockData)
        {
            return new StockDataDTO
            {
                FIGI = stockData.Identifier.FIGI,
                Bars = stockData.Bars.Values.Select(b => b.GetDTO()).ToList(),
                Info = stockData.Identifier.GetDTO(),
                StartDate = stockData.StartDate,
                EndDate = stockData.EndDate,
                Interval = stockData.Interval.GetTimeSpan()
            };
        }

        public static StockData GetModel(this StockDataDTO stockDataDTO)
        {
            var bars = stockDataDTO.Bars ?? new List<StockBarDTO>();

            return StockData.GetBuilder()
                .SetIdentifier(stockDataDTO.Info.GetModel())
                .SetInterval(stockDataDTO.Interval.GetInterval())
                .SetBars(bars.Select(b => b.GetModel()).ToList())
                .Build();
        }

        public static StockInfoDTO GetDTO(this StockIdentifier stockIdentifier)
        {
            return new StockInfoDTO
            {
                Country = stockIdentifier.Country,
                Currency = stockIdentifier.Currency,
                FIGI = stockIdentifier.FIGI,
                Name = stockIdentifier.Name,
                Sector = stockIdentifier.Name,
            };
        }

        public static StockIdentifier GetModel(this StockInfoDTO stockInfoDTO)
        {
            return new StockIdentifier(stockInfoDTO.Name, 
                stockInfoDTO.FIGI, 
                stockInfoDTO.Currency, 
                stockInfoDTO.Sector, 
                stockInfoDTO.Country);
        }
    }
}
