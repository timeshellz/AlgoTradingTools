using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTrading.TradeOptimizer.Interfaces
{
    public interface IStrategyBacktester
    {
        string ScriptName { get; set; }
        Dictionary<string, float> RunBacktest(Dictionary<string, float> testIndexes);
        Dictionary<string, float> GetStrategyIndexes();
    }
}
