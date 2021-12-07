using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoTrading.Neural
{
    public interface ISoftMaxActivatable : IActivatable
    {
        void Activate(double maxValue, double exponentSum);
    }
}
