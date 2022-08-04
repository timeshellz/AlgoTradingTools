using System;
using System.Collections.Generic;
using System.Text;
using AlgoTrading.Stocks;

namespace AlgoTrading.DQN.Benchmarking
{
    class BenchmarkCategory
    {
        public string Name { get; set; }
        public List<IntervalStockIdentifier> Stocks { get; set; }
    }
}
