using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTrading.TradeOptimizer.Interfaces
{
    public interface IIndexManager
    {
        Dictionary<string, int> IndexDefinitionLines { get; set; }
        Dictionary<string, float> GetFileIndexes();
        string ChangeIndexes(Dictionary<string, float> newIndexes, bool writeToFile = true);
    }
}
