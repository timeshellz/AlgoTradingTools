using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoTrading.Neural
{
    public interface ILinearActivatable : IActivatable
    {
        void Activate();
    }
}
