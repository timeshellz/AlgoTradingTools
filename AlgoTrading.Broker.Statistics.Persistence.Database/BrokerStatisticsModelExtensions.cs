using AlgoTrading.Broker.Statistics.Persistence.Database.DTO;
using AlgoTrading.Stocks;
using System.Collections.Generic;
using System.Linq;

namespace AlgoTrading.Broker.Statistics.Persistence.Database
{
    public static class BrokerStatisticsModelExtensions
    {
        public static BrokerSessionStatisticsDTO GetDTO(this BrokerSessionStatistics statistics)
        {
            return new BrokerSessionStatisticsDTO()
            {
                BestTradedStock = statistics.BestTradedStock.GetDTO(),
                TotalTradeDuration = statistics.TotalTradeDuration,
                TotalTradeProfit = statistics.TotalTradeProfit,
                TotalTrades = statistics.TotalTrades,
                StocksVisited = statistics.TotalStocksVisited,
            };
        }

        public static TradedStockStatisticsDTO GetDTO(this TradedStockStatistics statistics)
        {
            var model = new TradedStockStatisticsDTO()
            {
                StartDate = statistics.StartDate,
                EndDate = statistics.EndDate,
                StartPrice = statistics.StartPrice,
                EndPrice = statistics.EndPrice,
                Profit = statistics.Profit,
                TradeDuration = statistics.TradeDuration,
                Interval = statistics.Interval.GetTimeSpan()
            };

            if (statistics.PositionStatistics != null && statistics.PositionStatistics.Count > 0)
                model.Positions = statistics.PositionStatistics.Select(p => p.GetDTO()).ToList();
            else
                model.Positions = new List<MarketPositionStatisticsDTO>();

            return model;
        }

        public static MarketPositionStatisticsDTO GetDTO(this MarketPositionStatistics statistics)
        {
            return new MarketPositionStatisticsDTO()
            {
                Profit = statistics.Profit,
                Size = statistics.Size,
                StartDate = statistics.StartDate,
                EndDate = statistics.EndDate,
                StartPrice = statistics.StartPrice,
                EndPrice = statistics.EndPrice,
            };
        }

        public static BrokerSessionStatistics GetModel(this BrokerSessionStatisticsDTO statisticsDTO)
        {
            return new BrokerSessionStatistics()
            {
                TotalTradeDuration = statisticsDTO.TotalTradeDuration,
                BestTradedStock = statisticsDTO.BestTradedStock.GetModel(),
                TotalStocksVisited = statisticsDTO.StocksVisited,
                TotalTradeProfit = statisticsDTO.TotalTradeProfit,
                TotalTrades = statisticsDTO.TotalTrades
            };
        }

        public static TradedStockStatistics GetModel(this TradedStockStatisticsDTO statisticsDTO)
        {
            var model = new TradedStockStatistics()
            {
                StartDate = statisticsDTO.StartDate,
                EndDate = statisticsDTO.EndDate,
                StartPrice = statisticsDTO.StartPrice,
                EndPrice = statisticsDTO.EndPrice,
                Profit = statisticsDTO.Profit,
                TradeDuration = statisticsDTO.TradeDuration,
                Interval = statisticsDTO.Interval.GetInterval()
            };

            if (statisticsDTO.Positions != null && statisticsDTO.Positions.Count > 0)
                model.PositionStatistics = statisticsDTO.Positions.Select(p => p.GetModel()).ToList();
            else
                model.PositionStatistics = new List<MarketPositionStatistics>();

            return model;
        }

        public static MarketPositionStatistics GetModel(this MarketPositionStatisticsDTO statisticsDTO)
        {
            return new MarketPositionStatistics()
            {
                Profit = statisticsDTO.Profit,
                Size = statisticsDTO.Size,
                StartPrice = statisticsDTO.StartPrice,
                EndPrice = statisticsDTO.EndPrice,
                StartDate = statisticsDTO.StartDate,
                EndDate = statisticsDTO.EndDate,
            };       
        }
    }
}
