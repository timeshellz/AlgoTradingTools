using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;
using AlgoTrading.SimulatedBroker;
using AlgoTrading.Stocks;
using AlgoTrading.Neural;
using AlgoTrading.DQN;

namespace AlgoTrading
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //StockData stock = new StockData("ORCL", DateTime.Now - new TimeSpan(30000, 0, 0, 0), DateTime.Now, DataInterval.Day);
            //BrokerSettings settings = new BrokerSettings(3000, 0.03m);
            //BrokerEmulator emulator = new BrokerEmulator(stock, settings);

            //List<string> inputs = emulator.GetCurrentMarketState().Select(e => e.Key).ToList();
            //List<string> outputs = emulator.GetAllPossibleActions();

            //NeuralSettings neuralSettings = new NeuralSettings(inputs, outputs, 3, 0.1);
            //PrimaryDQNController controller = new DQNController("TestNetwork1", neuralSettings);

            //AgentSettings agentSettings = new AgentSettings(10000, 32, emulator, 0.95, 1, 0.1, 0.95);

            //controller.CreateAgent(agentSettings);
            //controller.Agent.RunEpoch(100);
        }
    }
}
