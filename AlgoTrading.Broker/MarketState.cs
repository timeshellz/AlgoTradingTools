using AlgoTrading.Broker.Positions;
using AlgoTrading.Stocks;
using System;
using System.Collections.Generic;

namespace AlgoTrading.Broker
{
    public class MarketState
    {
        public decimal Cash { get; set; }
        public decimal Equity { get; set; }
        public decimal EquityChange { get; set; }
        public StockBar PreviousStockBar { get; set; }
        public StockBar CurrentStockBar { get; set; }
        public IndicatorBar PreviousIndicators { get; set; }
        public IndicatorBar CurrentIndicators { get; set; }
        public MarketPosition PreviousPosition { get; set; }
        public MarketPosition CurrentPosition { get; set; }

        public MarketState(decimal cash, decimal equity, StockBar currentBar,
            IndicatorBar currentIndicators, MarketPosition currentPosition) : this(cash, equity, currentBar, currentIndicators)
        {
            CurrentPosition = currentPosition;

            if (CurrentPosition is OpenPosition open)
            {
                Equity = open.GetPositionValue(currentBar);
            }
        }

        public MarketState(decimal cash, decimal equity, StockBar currentBar,
            IndicatorBar currentIndicators)
        {
            Cash = cash;
            Equity = equity;
            CurrentStockBar = currentBar;
            CurrentIndicators = currentIndicators;
        }

        private MarketState() { }

        public NextMarketStateBuilder PrepareNext(StockBar nextBar, IndicatorBar nextIndicators)
        {
            return new NextMarketStateBuilder(this, nextBar, nextIndicators);
        }

        public static List<string> GetPossibleValues()
        {
            return new List<string>()
            {
                "Cash",
                "Equity",
                "High",
                "Low",
                "Open",
                "Close",
                "Volume",
                "Stochastic",
                "StochasticSignal",
                "RSI",
                "SMA200",
                "SMA50",
                "DEMA",
                "DEMAROC",
                "MACD",
                "MACDSignal",
                "MACDHistogram",
                "BBHigh",
                "BBMid",
                "BBLow",
                "OBVROC",
                "ADX",
                "AroonUp",
                "AroonDown",
                "ROC",
                "TripleDerivative",
            };
        }

        public class NextMarketStateBuilder
        {
            private MarketState previousMarketState;
            private MarketState newMarketState;

            public NextMarketStateBuilder(MarketState previousState, StockBar nextBar, IndicatorBar nextIndicators)
            {
                previousMarketState = previousState;
                newMarketState = new MarketState()
                {
                    Cash = previousMarketState.Cash,
                    Equity = previousMarketState.Equity,
                    PreviousPosition = previousState.CurrentPosition,
                };

                if (!(previousMarketState.CurrentPosition is ClosedPosition))
                {
                    newMarketState.CurrentPosition = previousMarketState.CurrentPosition;
                    newMarketState.PreviousStockBar = previousState.CurrentStockBar;
                    newMarketState.CurrentStockBar = nextBar;
                    newMarketState.PreviousIndicators = previousState.CurrentIndicators;
                    newMarketState.CurrentIndicators = nextIndicators;
                }
                else
                {
                    newMarketState.CurrentPosition = null;
                    newMarketState.PreviousStockBar = previousState.PreviousStockBar;
                    newMarketState.CurrentStockBar = previousMarketState.CurrentStockBar;
                    newMarketState.PreviousIndicators = previousState.PreviousIndicators;
                    newMarketState.CurrentIndicators = previousMarketState.CurrentIndicators;
                }

                if (newMarketState.CurrentPosition != null)
                    newMarketState.Equity = newMarketState.CurrentPosition.GetPositionValue(newMarketState.CurrentStockBar);
            }

            public NextMarketStateBuilder CanEnterPosition(MarketPosition newPosition, out bool value)
            {
                OpenPosition result;
                value = TryCreateOpenPosition(newPosition, out result);

                return this;
            }

            public NextMarketStateBuilder CanExitPosition(out bool value)
            {
                ClosedPosition result;

                value = TryCreateClosedPosition(out result);

                return this;
            }

            private bool TryCreateOpenPosition(MarketPosition position, out OpenPosition openPosition)
            {
                openPosition = null;

                openPosition = position.Open(newMarketState.CurrentStockBar);

                if (newMarketState.CurrentPosition != null && !(newMarketState.CurrentPosition is ClosedPosition))
                    return false;

                if (newMarketState.Cash - openPosition.GetTotalOpenCharge() < 0)
                    return false;

                return true;
            }

            private bool TryCreateClosedPosition(out ClosedPosition closedPosition)
            {
                closedPosition = null;

                if (newMarketState.CurrentPosition == null)
                    return false;

                if (newMarketState.CurrentPosition is OpenPosition openPosition)
                    closedPosition = openPosition.Close(newMarketState.CurrentStockBar);
                else
                    return false;

                return true;
            }

            public NextMarketStateBuilder EnterNewPosition(MarketPosition newPosition)
            {
                OpenPosition openPosition;

                if (!TryCreateOpenPosition(newPosition, out openPosition))
                    throw new MarketStateException("Can't enter new position.");

                newMarketState.CurrentPosition = openPosition;
                newMarketState.Cash -= openPosition.GetTotalOpenCharge();
                newMarketState.Equity = openPosition.GetPositionValue(newMarketState.CurrentStockBar);

                return this;
            }

            public NextMarketStateBuilder ExitOpenPosition(out ClosedPosition closedPosition)
            {
                ClosedPosition position;
                if (TryCreateClosedPosition(out position))
                {
                    decimal positionGain = position.GetTotalClosingCharge();

                    newMarketState.CurrentPosition = position;
                    newMarketState.Cash += positionGain;
                    newMarketState.Equity = 0;

                    closedPosition = position;
                    return this;
                }
                else
                    throw new ArgumentException("Can't exit position.");
            }

            public MarketState GetNextState()
            {
                return newMarketState;
            }
        }
    }

    public static class MarketStateExtensions
    {
        public static Dictionary<string, double> ToDictionary(this MarketState state)
        {
            return new Dictionary<string, double>()
            {
                ["Cash"] = (double)state.Cash,
                ["Equity"] = (double)state.Equity,
                ["High"] = (double)state.CurrentStockBar.High,
                ["Low"] = (double)state.CurrentStockBar.Low,
                ["Open"] = (double)state.CurrentStockBar.Open,
                ["Close"] = (double)state.CurrentStockBar.Close,
                ["Volume"] = (double)state.CurrentStockBar.Volume,
                ["Stochastic"] = (double)(state.CurrentIndicators.Stochastic ?? 0),
                ["StochasticSignal"] = (double)(state.CurrentIndicators.StochasticSignal ?? 0),
                ["RSI"] = (double)(state.CurrentIndicators.RSI),
                ["SMA200"] = (double)(state.CurrentIndicators.SMA200 ?? 0),
                ["SMA50"] = (double)(state.CurrentIndicators.SMA50 ?? 0),
                ["DEMA"] = (double)(state.CurrentIndicators.DEMA ?? 0),
                ["DEMAROC"] = (double)(state.CurrentIndicators.DEMAROC ?? 0),
                ["MACD"] = (double)(state.CurrentIndicators.MACD ?? 0),
                ["MACDSignal"] = (double)(state.CurrentIndicators.MACDSignal ?? 0),
                ["MACDHistogram"] = (double)(state.CurrentIndicators.MACDHistogram ?? 0),
                ["BBHigh"] = (double)(state.CurrentIndicators.BBHigh ?? 0),
                ["BBMid"] = (double)(state.CurrentIndicators.BBMid ?? 0),
                ["BBLow"] = (double)(state.CurrentIndicators.BBLow ?? 0),
                ["OBVROC"] = (double)(state.CurrentIndicators.OBVROC ?? 0),
                ["ADX"] = (double)(state.CurrentIndicators.ADX ?? 0),
                ["AroonUp"] = (double)(state.CurrentIndicators.AroonUp ?? 0),
                ["AroonDown"] = (double)(state.CurrentIndicators.AroonDown ?? 0),
                ["ROC"] = (double)(state.CurrentIndicators.ROC),
                ["TripleDerivative"] = (double)(state.CurrentIndicators.TripleDerivative ?? 0),
            };
        }
    }

    public class MarketStateException : Exception
    {
        public MarketStateException(string message) : base(message) { }
    }
}
