using System;
using System.Collections.Generic;
using AlgoTrading.TradeOptimizer.Interfaces;
using AlgoTrading.Stocks;
using AlgoTrading.SimulatedBroker;
using AlgoTrading.Neural;
using System.Linq;

namespace AlgoTrading.TradeOptimizer.Backtesters.Native
{
    class NeuralBacktester : INeuralBacktester
    {
        public BrokerEmulator Emulator { get; set; }
        public string NeuralNetworkFile { get; set; }

        public void RunBacktest(DQNController controller)
        {
            Dictionary<string, double> state = Emulator.GetCurrentMarketState();
            Dictionary<string, double> rewardVars = Emulator.GetCurrentTraderStatistics();
            string action = controller.PerformStateActionTransition(state, controller.CalculateReward(rewardVars));

            switch(action)
            {
                case "Buy":
                    Emulator.Purchase(4);
                    break;
                case "Sell":
                    Emulator.Sell();
                    break;
            }

            Emulator.AdvanceTime();
        }
    }
}
