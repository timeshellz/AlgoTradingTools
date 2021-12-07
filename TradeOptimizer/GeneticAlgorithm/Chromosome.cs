using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;

namespace AlgoTrading.TradeOptimizer.Genetics
{
    public class Chromosome
    {
        public Gene[] Genes { get; set; }

        public Chromosome(Gene[] genes)
        {
            Genes = genes;
        }
    }
}
