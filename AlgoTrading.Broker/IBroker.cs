using System;
using System.Collections.Generic;
using AlgoTrading.Stocks;
using System.Threading.Tasks;
using System.Linq;
using AlgoTrading.Statistics;

namespace AlgoTrading.Broker
{
    public interface IBroker : IStatisticsProvider<BrokerSessionStatistics>
    {
        StockData SelectedStockData { get; }
        MarketState CurrentState { get; }
        Task Start();
        Task<MarketState> GetNextTimestep();       
        List<BrokerAction> GetAvailableActions();
        void EnterPosition(int positionSize);
        void ExitPosition();
        int GetLimitedPositionSize(double percentage);
    }

    public enum BrokerAction { Skip, Long75, Long50, Long25, Short75, Short50, Short25, Close}

    public static class BrokerActionExtensions
    {
        public static string GetActionString(this BrokerAction action)
        {
            switch(action)
            {
                case BrokerAction.Long75:
                    return "Long75";
                case BrokerAction.Long50:
                    return "Long50";
                case BrokerAction.Long25:
                    return "Long25";
                case BrokerAction.Short75:
                    return "Short75";
                case BrokerAction.Short50:
                    return "Short50";
                case BrokerAction.Short25:
                    return "Short25";
                case BrokerAction.Skip:
                    return "Skip";
                case BrokerAction.Close:
                    return "Close";
            }

            return "";
        }

        public static List<string> GetActionStrings(this List<BrokerAction> actions)
        {
            return new List<string>(actions.Select(e => e.GetActionString()));
        }

        public static BrokerAction GetBrokerAction(this string action)
        {
            switch(action)
            {
                case "Long75":
                    return BrokerAction.Long75;
                case "Long50":
                    return BrokerAction.Long50;
                case "Long25":
                    return BrokerAction.Long25;
                case "Short75":
                    return BrokerAction.Short75;
                case "Short50":
                    return BrokerAction.Short50;
                case "Short25":
                    return BrokerAction.Short25;
                case "Skip":
                    return BrokerAction.Skip;
                case "Close":
                    return BrokerAction.Close;
            }

            throw new ArgumentException("Broker action not found.");
        }

        public static List<BrokerAction> GetBrokerActions(this List<string> actions)
        {
            return new List<BrokerAction>(actions.Select(e => e.GetBrokerAction()));
        }
    }
}
