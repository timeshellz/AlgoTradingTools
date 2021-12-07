using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTrading.TradeOptimizer.Genetics
{
    public class Gene
    {
        public string Name { get; private set; }
        float vvalue = 0.5f;
        public float Value { get; set; }
        public Gene(string name, float value)
        {
            Name = name;
            Value = value;
        }
    }
}
