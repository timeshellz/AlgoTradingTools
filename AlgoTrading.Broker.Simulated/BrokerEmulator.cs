using AlgoTrading.Stocks;
using AlgoTrading.Broker;
using System;
using System.Collections.Generic;
using AlgoTrading.Stocks.Persistence;
using System.Threading.Tasks;
using System.Linq;

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

            PrepareNextState();

            if (!CanExecuteActions 
                && (sessionStatistics.BestTradedStock == null 
                || sessionStatistics.BestTradedStock != null 
                && sessionStatistics.BestTradedStock.Profit < currentStockStatistics.Profit))
                sessionStatistics.BestTradedStock = currentStockStatistics;

            return Task.FromResult(CurrentState);
        }

        private void PrepareNextState()
        {
            if (stockBarsEnumerator.MoveNext() && indicatorBarsEnumerator.MoveNext())
                nextStateBuilder = CurrentState.PrepareNext(stockBarsEnumerator.Current.Value, indicatorBarsEnumerator.Current.Value);
            else
                nextStateBuilder = null;
        }

        public Task<MarketState> GetBeginningTimestep()
        {
            currentStockStatistics = new TradedStockStatistics();
            currentStockStatistics.StockData = SelectedStockData;

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
                nextStateBuilder.EnterNewPosition(new MarketPosition(positionSize, Configuration.Commission));
        }

        public void ExitPosition()
        {
            if (nextStateBuilder != null)
            {
                nextStateBuilder.ExitOpenPosition(out ClosedPosition position);

                sessionStatistics.TotalTradeDuration += position.GetPositionDuration();
                currentStockStatistics.TradeDuration += position.GetPositionDuration();

                sessionStatistics.TotalTrades++;
                currentStockStatistics.Positions.Add(position);

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

                bool can75 = false, can50 = false, can25 = false;
                bool canExit;

                int size75 = GetLimitedPositionSize(0.75d);
                int size50 = GetLimitedPositionSize(0.50d);
                int size25 = GetLimitedPositionSize(0.25d);

                if(size75 != 0)
                    nextStateBuilder.CanEnterPosition(
                            new MarketPosition(size75, Configuration.Commission), out can75);

                if(size50 != 0)
                    nextStateBuilder.CanEnterPosition(
                        new MarketPosition(size50, Configuration.Commission), out can50);

                if(size25 != 0)
                    nextStateBuilder.CanEnterPosition(
                        new MarketPosition(size25, Configuration.Commission), out can25);

                nextStateBuilder.CanExitPosition(out canExit);

                if (can75)
                {
                    result.Add(BrokerAction.Long75);
                    result.Add(BrokerAction.Short75);
                }
                if(can50)
                {
                    result.Add(BrokerAction.Long50);
                    result.Add(BrokerAction.Short50);
                }                    
                if(can25)
                {
                    result.Add(BrokerAction.Long25);
                    result.Add(BrokerAction.Short25);
                }

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
            double cashLimit = (double)CurrentState.Cash * percentage;

            return (int)(cashLimit / (double)CurrentState.CurrentStockBar.Close);
        }

        private async Task SelectRandomStock()
        {            
            int stockIndex = 0;

            if(Configuration.StockIdentifiers.Count > 0)
                stockIndex = stockRandomizer.Next(0, Configuration.StockIdentifiers.Count);

            SelectedStockData = await persistenceManager.LoadStockData(Configuration.StockIdentifiers.ElementAt(stockIndex));
            SelectedStockData.SetIndicators(indicatorProvider);            
        }
    }
}
