using AlgoTrading.Broker.Positions;
using AlgoTrading.Broker.Statistics;
using AlgoTrading.Stocks;
using AlgoTrading.Stocks.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoTrading.Broker.Simulated
{
    public class BrokerEmulator : IHistoricalBroker
    {
        public StockData SelectedStockData { get; private set; }
        private IEnumerator<KeyValuePair<DateTime, StockBar>> stockBarsEnumerator;
        private IEnumerator<KeyValuePair<DateTime, IndicatorBar>> indicatorBarsEnumerator;

        public BrokerConfiguration Configuration { get; private set; }

        private BrokerSessionStatistics sessionStatistics = new BrokerSessionStatistics();
        private TradedStockStatistics currentStockStatistics = new TradedStockStatistics();

        public MarketState CurrentState { get; private set; }
        private MarketState.NextMarketStateBuilder nextStateBuilder;

        private IStockPersistenceManager persistenceManager;
        private IIndicatorProvider indicatorProvider;

        private Random stockRandomizer = new Random();

        private bool CanExecuteActions
        {
            get => nextStateBuilder != null && CurrentState != null && Configuration != null
                && CurrentState.Cash + CurrentState.Equity - (CurrentState.Equity * Configuration.Commission) >= Configuration.StartCapital * 0.5M;
        }

        public BrokerEmulator(BrokerConfiguration configuration, IStockPersistenceManager persistence,
            IIndicatorProvider indicators)
        {
            persistenceManager = persistence;
            indicatorProvider = indicators;
            Configuration = configuration;

            sessionStatistics = new BrokerSessionStatistics();
        }

        public async Task Start()
        {
            await SelectRandomStock();
            await GetBeginningTimestep();
        }

        public Task<MarketState> GetNextTimestep()
        {
            CurrentState = nextStateBuilder.GetNextState();

            PrepareNextState(CurrentState.PreviousStockBar != CurrentState.CurrentStockBar);

            if (!CanExecuteActions
                && (sessionStatistics.BestTradedStock == null
                || sessionStatistics.BestTradedStock != null
                && sessionStatistics.BestTradedStock.Profit < currentStockStatistics.Profit))
                sessionStatistics.BestTradedStock = currentStockStatistics;

            return Task.FromResult(CurrentState);
        }

        private void PrepareNextState(bool advanceTime = true)
        {
            if (advanceTime)
            {
                if (!(stockBarsEnumerator.MoveNext() && indicatorBarsEnumerator.MoveNext()))
                {
                    nextStateBuilder = null;
                    return;
                }
            }

            nextStateBuilder = CurrentState.PrepareNext(stockBarsEnumerator.Current.Value, indicatorBarsEnumerator.Current.Value);
        }

        public Task<MarketState> GetBeginningTimestep()
        {
            currentStockStatistics = new TradedStockStatistics();
            currentStockStatistics.StockName = SelectedStockData.Identifier.Name;
            currentStockStatistics.StartDate = SelectedStockData.StartDate;
            currentStockStatistics.EndDate = SelectedStockData.EndDate;
            currentStockStatistics.StartPrice = SelectedStockData.Bars.First().Value.Close;
            currentStockStatistics.EndPrice = SelectedStockData.Bars.Last().Value.Close;
            currentStockStatistics.Interval = SelectedStockData.Identifier.Interval;

            stockBarsEnumerator = SelectedStockData.Bars.GetEnumerator();
            indicatorBarsEnumerator = SelectedStockData.Indicators.GetEnumerator();

            stockBarsEnumerator.MoveNext();
            indicatorBarsEnumerator.MoveNext();

            CurrentState = new MarketState(Configuration.StartCapital, 0,
                stockBarsEnumerator.Current.Value, indicatorBarsEnumerator.Current.Value);

            PrepareNextState();
            GetNextTimestep();

            return Task.FromResult(CurrentState);
        }

        public void EnterPosition(int positionSize)
        {
            if (nextStateBuilder != null)
            {
                nextStateBuilder.EnterNewPosition(new MarketPosition(positionSize, Configuration.Commission));
            }
        }

        public void ExitPosition()
        {
            if (nextStateBuilder != null)
            {
                nextStateBuilder.ExitOpenPosition(out ClosedPosition position);

                sessionStatistics.TotalTradeDuration += position.GetPositionDuration();
                currentStockStatistics.TradeDuration += position.GetPositionDuration();

                sessionStatistics.TotalTrades++;
                currentStockStatistics.PositionStatistics.Add(new MarketPositionStatistics(position));

                sessionStatistics.TotalTradeProfit += position.CommissionedProfit;
                currentStockStatistics.Profit += position.CommissionedProfit;
            }
        }

        public List<BrokerAction> GetAvailableActions()
        {
            if (CanExecuteActions)
            {
                var result = new List<BrokerAction>();
                result.Add(BrokerAction.Skip);

                bool canLong75 = false, canLong50 = false, canLong25 = false, canShort75 = false, canShort50 = false, canShort25 = false;
                bool canExit;

                int size75 = GetLimitedPositionSize(0.75d);
                int size50 = GetLimitedPositionSize(0.50d);
                int size25 = GetLimitedPositionSize(0.25d);

                if (size75 != 0)
                {
                    nextStateBuilder.CanEnterPosition(
                            new MarketPosition(size75, Configuration.Commission), out canLong75);
                    nextStateBuilder.CanEnterPosition(
                            new MarketPosition(-1 * size75, Configuration.Commission), out canShort75);
                }

                if (size50 != 0)
                {
                    nextStateBuilder.CanEnterPosition(
                        new MarketPosition(size50, Configuration.Commission), out canLong50);
                    nextStateBuilder.CanEnterPosition(
                        new MarketPosition(-1 * size50, Configuration.Commission), out canShort50);
                }

                if (size25 != 0)
                {
                    nextStateBuilder.CanEnterPosition(
                        new MarketPosition(size25, Configuration.Commission), out canLong25);

                    nextStateBuilder.CanEnterPosition(
                        new MarketPosition(-1 * size25, Configuration.Commission), out canShort25);
                }

                nextStateBuilder.CanExitPosition(out canExit);

                if (canLong75)
                    result.Add(BrokerAction.Long75);
                if (canLong50)
                    result.Add(BrokerAction.Long50);
                if (canLong25)
                    result.Add(BrokerAction.Long25);
                if (canShort75)
                    result.Add(BrokerAction.Short75);
                if (canShort50)
                    result.Add(BrokerAction.Short50);
                if (canShort25)
                    result.Add(BrokerAction.Short25);

                if (canExit)
                    result.Add(BrokerAction.Close);

                return result;
            }
            else
                return new List<BrokerAction>();
        }

        public BrokerSessionStatistics GetStatistics()
        {
            return sessionStatistics;
        }

        public void ResetStatistics()
        {
            sessionStatistics = new BrokerSessionStatistics();
            currentStockStatistics = new TradedStockStatistics();
        }

        public int GetLimitedPositionSize(double percentage)
        {
            double cashLimit = (double)(CurrentState.Cash + CurrentState.Equity) * percentage;

            return (int)(cashLimit / (double)CurrentState.CurrentStockBar.Close);
        }

        private async Task SelectRandomStock()
        {
            int stockIndex = 0;

            if (Configuration.StockIdentifiers.Count > 0)
                stockIndex = stockRandomizer.Next(0, Configuration.StockIdentifiers.Count);

            SelectedStockData = await persistenceManager.LoadStockData(Configuration.StockIdentifiers.ElementAt(stockIndex));
            SelectedStockData.SetIndicators(indicatorProvider);

            sessionStatistics.TotalStocksVisited++;
        }
    }
}
