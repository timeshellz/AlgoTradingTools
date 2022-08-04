using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using AlgoTrading.DQN;
using AlgoTrading.Agent.Benchmarking;

namespace AlgoTrading.DQN.Benchmarking
{
    public class BenchmarkingAgentDirector : TradingAgentDirector
    {
        private BenchmarkingDirectorStatistics statistics = new BenchmarkingDirectorStatistics();

        new public BenchmarkingAgent Agent { get; private set; }

        public BenchmarkingAgentDirector(BenchmarkingAgent agent) : base(agent)
        {
            Agent = agent;
        }

        public async Task DirectBenchmark()
        {

        }
    }
}
