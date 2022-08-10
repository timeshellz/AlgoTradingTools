using AlgoTrading.Agent.Benchmarking;
using AlgoTrading.DQN.Statistics;
using System.Threading.Tasks;

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
