using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Skender.Stock.Indicators;
using Cyotek.Collections.Generic;
using AlgoTrading.Stocks;
using AlgoTrading.Utilities;
using Newtonsoft.Json;

namespace AlgoTrading.SimulatedBroker
{
    public class BrokerEmulator
    {
        public static Dictionary<DataInterval, List<StockData>> LoadedStocks { get; private set; }
        public static StockDataController StockDataController { get; private set; }
        public int TrackedStockID { get; private set; }
        public StockData TrackedStock { get; private set; }  
        public int MinimumIndicatorTime { get; private set; }
        public int CurrentBarPos { get; private set; }
        public StockBar PreviousBar { get; private set; }
        public StockBar CurrentBar { get; private set; }
        public Dictionary<string, double> CurrentIndicatorValues { get; private set; }
        public decimal Cash { get; private set; }
        public decimal Equity { get; private set; }
        public decimal CashChange { get; private set; }
        public decimal EquityChange { get; private set; }
        public BrokerSettings Settings { get; private set; }
        public List<Position> FinishedTrades { get; private set; }
        public Position CurrentPosition { get; private set; }
        public bool IsTerminalCondition { get; private set; }
        public bool IsDataOver { get; private set; }
        public bool IsLastActionValid { get; private set; }

        public TimeSpan TotalTradeDuration { get; private set; }
        public int TotalTradeCount { get; private set; }
        public decimal TotalTradeProfit { get; private set; }
        public decimal TotalCommissionedProfit { get; private set; }

        public BrokerEmulator(BrokerSettings settings)
        {
            TrackedStockID = 0;
            StockDataController = new StockDataController();
            //StockDataController.LoadSavedStockData(settings.StockNames);
            TrackedStock = StockDataController.Stocks.Values.First().First();
            LoadedStocks = StockDataController.Stocks;
            Settings = settings;

            //SynchronizeStockLengths();
            Setup(settings);
        }

        void SynchronizeStockLengths()
        {
            foreach(List<StockData> stockDatas in LoadedStocks.Values)
            {
                int minLength = int.MaxValue;

                foreach(StockData stock in stockDatas)
                {
                    if (stock.Bars.Count < minLength)
                        minLength = stock.Bars.Count;
                }

                foreach(StockData stock in stockDatas)
                {
                    //stock.Bars = stock.Bars.Where(e => e.Key < minLength).ToDictionary(s => s.Key, s => s.Value);

                    stock.TotalBars = stock.Bars.Count;
                }
            }
        }

        void Setup(BrokerSettings settings)
        {
            Cash = settings.StartCapital; ;         
            FinishedTrades = new List<Position>();

            GetMinimumIndicatorTime();
            SetBeginning();
        }

        void GetMinimumIndicatorTime()
        {
            MinimumIndicatorTime = int.MaxValue;

            foreach (IEnumerable<IResult> results in TrackedStock.Indicators.Values)
            {
                int resultCount = results.Count();

                if (resultCount < MinimumIndicatorTime)
                    MinimumIndicatorTime = resultCount;
            }
        }

        void SetBeginning()
        {            
            //CurrentBarPos = TrackedStock.TotalBars - MinimumIndicatorTime + 1;
            //CurrentBar = TrackedStock.Bars[CurrentBarPos];
            //PreviousBar = TrackedStock.Bars[CurrentBarPos - 1];
        }

        public void AdvanceTime()
        {           
            if (CurrentBarPos + 1 == TrackedStock.TotalBars)
            {
                IsDataOver = true;
                return;
            }

            CurrentBarPos++;

            UpdatePosition();
            CashChange = 0;
            EquityChange = 0;
            Equity = 0;

            if(CurrentPosition != null && !CurrentPosition.IsComplete)
            {
                EquityChange = CurrentPosition.GetCommissionedEquityChange(PreviousBar);
                Equity = CurrentPosition.GetPositionEquity(PreviousBar);
            }

            if ((Cash + Equity) <= 0)
                IsTerminalCondition = true;
        }

        public bool Purchase(int amount)
        {
            if (CanBuy())
            {               
                if (IsPurchasePossible(amount))
                {
                    Position newPosition = new Position();

                    //CashChange = newPosition.Enter(TrackedStock.Name, amount, PreviousBar, Settings.Commission);
                    Cash -= CashChange;
                    CurrentPosition = newPosition;

                    IsLastActionValid = true;
                    return true;
                }
            }

            IsLastActionValid = false;
            return false;
        }

        bool CanBuy()
        {
            if (!IsPurchasePossible(3))
                return false;

            if (CurrentPosition == null || CurrentPosition.IsComplete)
                return true;
            else
                return false;
        }

        public bool Sell()
        {
            if(CanSell())
            {
                CashChange = CurrentPosition.ExitAll(PreviousBar, Settings.Commission);
                Cash += CashChange;

                TotalTradeDuration += PreviousBar.Date - CurrentPosition.StartDate;
                TotalTradeCount++;
                TotalTradeProfit += CurrentPosition.Profit;
                TotalCommissionedProfit += CurrentPosition.CommissionedProfit;

                IsLastActionValid = true;
                return true;
            }
            else
            {
                IsLastActionValid = false;
                return false;
            }
                            
        }

        bool CanSell()
        {
            //if (CurrentPosition != null && !CurrentPosition.IsComplete && TrackedStock.Name == CurrentPosition.StockName)
            //    return true;
            //else
                return false;
        }

        public bool Hold()
        {
            IsLastActionValid = true;
            return true;
        }

        public void TrackNextStock()
        {
            if (!(CurrentPosition is null))
            {
                if (!CurrentPosition.IsComplete)
                {
                    IsLastActionValid = false;
                    return;
                }
            }

            if (TrackedStockID != LoadedStocks[TrackedStock.Interval].Count - 1)
            {
                TrackedStock = LoadedStocks[TrackedStock.Interval].ElementAt(TrackedStockID + 1);
                TrackedStockID++;
            }
            else
            {
                TrackedStock = LoadedStocks[TrackedStock.Interval].ElementAt(0);
                TrackedStockID = 0;
            }

            IsLastActionValid = true;

            UpdatePosition();
        }

        public void TrackPreviousStock()
        {
            if(!(CurrentPosition is null))
            {
                if(!CurrentPosition.IsComplete)
                {
                    IsLastActionValid = false;
                    return;
                }
            }

            if (TrackedStockID != 0)
            {
                TrackedStock = LoadedStocks[TrackedStock.Interval].ElementAt(TrackedStockID - 1);
                TrackedStockID--;
            }
            else
            {
                TrackedStock = LoadedStocks[TrackedStock.Interval].ElementAt(LoadedStocks[TrackedStock.Interval].Count - 1);
                TrackedStockID = LoadedStocks[TrackedStock.Interval].Count - 1;
            }

            IsLastActionValid = true;

            UpdatePosition();
        }

        public void Reset()
        {
            Cash = Settings.StartCapital;          
            CurrentPosition = null;
            IsTerminalCondition = false;
            IsDataOver = false;

            SetBeginning();
            ResetSessionStatistics();
        }        

        public void TrackRandomStock()
        {
            int randomInterval = (int)RandomGenerator.Generate(0, LoadedStocks.Keys.Count);
            int randomStockNumber = (int)RandomGenerator.Generate(0, LoadedStocks.Values.ElementAt(randomInterval).Count);
            TrackedStock = LoadedStocks.Values.ElementAt(randomInterval).ElementAt(randomStockNumber);

            TrackedStockID = randomStockNumber;

            SetBeginning();
        }

        public void TrackNextStock(int stockNumber, DataInterval interval)
        {
            TrackedStock = LoadedStocks[interval].ElementAt(stockNumber);
            TrackedStockID = stockNumber;

            IsLastActionValid = true;

            UpdatePosition();
        }

        void UpdatePosition()
        {
            //CurrentBar = TrackedStock.Bars[CurrentBarPos];

            //if(CurrentBarPos != 0)
            //{
            //    PreviousBar = TrackedStock.Bars[CurrentBarPos - 1];
            //}         
        }

        bool IsPurchasePossible(int amount)
        {
            if (PreviousBar.Close * amount > Cash)
                return false;
            else
                return true;
        }

        public BrokerTimestepStatistics GetTimestepStatistics()
        {
            int positionSize = 0;
            double positionValue = 0;

            if (!(CurrentPosition is null))
            {
                positionValue = (double)CurrentPosition.TotalCommissionedStartPrice;
            }
                

            return new BrokerTimestepStatistics(Cash, Equity, Settings.StartCapital, EquityChange, positionValue, IsLastActionValid);
        }

        public BrokerSessionStatistics GetSessionStatistics()
        {
            return new BrokerSessionStatistics(TotalTradeDuration.TotalHours, (double)(TotalCommissionedProfit), TotalTradeCount);
        }

        public void ResetSessionStatistics()
        {
            TotalTradeCount = 0;
            TotalTradeDuration = new TimeSpan(0,0,0);
            TotalTradeProfit = 0;
            TotalCommissionedProfit = 0;
        }

        public Dictionary<string, double> GetCurrentMarketState()
        {
            Dictionary<string, double> output = new Dictionary<string, double>();
            Dictionary<string, IResult> currentIndicators = new Dictionary<string, IResult>();

            foreach (KeyValuePair<string, IEnumerable<IResult>> indicator in TrackedStock.Indicators)
            {
                currentIndicators.Add(indicator.Key, indicator.Value.Where(e => e.Date == CurrentBar.Date).DefaultIfEmpty(
                    (IResult)Activator.CreateInstance(indicator.Value.First().GetType())).First());
            }

            int positionSize = 0;

            if (CurrentPosition != null)
                positionSize = CurrentPosition.CurrentSize;

            output.Add("PositionSize", (double)positionSize);
            output.Add("Cash", (double)Cash);
            output.Add("Equity", (double)Equity);
            output.Add("High", (double)CurrentBar.High);
            output.Add("Low", (double)CurrentBar.Low);
            output.Add("Open", (double)CurrentBar.Open);
            output.Add("Close", (double)CurrentBar.Close);
            output.Add("Volume", (double)CurrentBar.Volume / 100000);
            output.Add("Stochastic", (double?)((StochResult)currentIndicators["Stochastic"]).Oscillator ?? 0d);
            output.Add("StochasticSignal", (double?)((StochResult)currentIndicators["Stochastic"]).Signal ?? 0d);
            output.Add("Sma200", (double?)((SmaResult)currentIndicators["Sma200"]).Sma ?? 0d);
            output.Add("Sma50", (double?)((SmaResult)currentIndicators["Sma50"]).Sma ?? 0d);
            output.Add("Dema", (double?)((DemaResult)currentIndicators["Dema"]).Dema ?? 0d);
            output.Add("DemaROC", (double?)((RocResult)currentIndicators["DemaROC"]).Roc ?? 0d);
            output.Add("MACD", (double?)((MacdResult)currentIndicators["MACD"]).Macd ?? 0d);
            output.Add("MACDSig", (double?)((MacdResult)currentIndicators["MACD"]).Signal ?? 0d);
            output.Add("MACDHisto", (double?)((MacdResult)currentIndicators["MACD"]).Histogram ?? 0d);
            output.Add("BBHigh", (double?)((BollingerBandsResult)currentIndicators["BB"]).UpperBand ?? 0d);
            output.Add("BBMid", (double?)((BollingerBandsResult)currentIndicators["BB"]).Sma ?? 0d);
            output.Add("BBLow", (double?)((BollingerBandsResult)currentIndicators["BB"]).LowerBand ?? 0d);
            output.Add("OBVROC", (double?)((RocResult)currentIndicators["OBVROC"]).Roc ?? 0d);
            output.Add("ADX", (double?)((AdxResult)currentIndicators["ADX"]).Adx ?? 0d);
            output.Add("AroonUp", (double?)((AroonResult)currentIndicators["Aroon"]).AroonUp ?? 0d);
            output.Add("AroonDown", (double?)((AroonResult)currentIndicators["Aroon"]).AroonDown ?? 0d);
            output.Add("DDD", (double?)((RocResult)currentIndicators["DDD"]).Roc ?? 0d);

            return output;
        }

        public List<string> GetCurrentPossibleActions()
        {
            List<string> actions = new List<string>();

            actions.Add("Hold");

            if (CanSell())
                actions.Add("Sell");
            if (CanBuy())
                actions.Add("Buy");

            return actions;
        }

        public static List<string> GetAllPossibleOutputs()
        {
            List<string> actions = new List<string>()
            {
                "PositionSize", "Cash", "Equity", "High", "Low", "Open", "Close", "Volume", "Stochastic", "StochasticSignal",
                "Sma200", "Sma50", "Dema", "DemaROC", "MACD", "MACDSig", "MACDHisto", "BBHigh", "BBMid", "BBLow", "OBVROC", "ADX", "AroonUp",
                "AroonDown", "DDD"
            };

            return actions;
        }

        public static List<string> GetAllPossibleActions()
        {
            List<string> actions = new List<string>()
            {
                "Buy", "Sell", "Hold"//, "TrackNext", "TrackLast"
            };

            return actions;
        }
        
    }

    public class BrokerSettings
    {
        [JsonProperty]
        public List<StockIdentifier> StockNames { get; set; } = new List<StockIdentifier>();
        [JsonProperty]
        public decimal StartCapital { get; set; } = 600;
        [JsonProperty]
        public decimal Commission { get; set; } = 0.0003M;

        public BrokerSettings() { }
    }
}
