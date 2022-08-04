using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlgoTrading.Agent;
using AlgoTrading.Broker;
using AlgoTrading.Neural;
using AlgoTrading.Statistics;

namespace AlgoTrading.Agent.Benchmarking
{
    public class BenchmarkingAgent : TradingAgent
    {
        public BenchmarkingAgentConfiguration Configuration { get; private set; }
       
        public BenchmarkingAgent(BenchmarkingAgentConfiguration configuration, NeuralNetwork network, IBroker broker) : base(network, broker)
        {
            Configuration = configuration;
        }
    }
}
