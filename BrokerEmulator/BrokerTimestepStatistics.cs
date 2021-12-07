using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoTrading.SimulatedBroker
{
    public struct BrokerTimestepStatistics
    {
        public double Cash { get; private set; }
        public double Equity { get; private set; }
        public double StartingCash { get; private set; }
        public double EquityChange { get; private set; }
        public double PositionStartValue { get; private set; }
        public bool IsLastActionValid { get; private set; }

        public BrokerTimestepStatistics(decimal cash, decimal equity, decimal startingCash, decimal equityChange, double positionStartValue, bool isLastActionValid)
        {
            Cash = (double)cash;
            Equity = (double)equity;
            StartingCash = (double)startingCash;
            EquityChange = (double)equityChange;
            PositionStartValue = positionStartValue;
            IsLastActionValid = isLastActionValid;
        }
    }
}
