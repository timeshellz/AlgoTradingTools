using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoTrading.Statistics
{
    public interface IStatisticsProvider<T> where T : IStatistics
    {
        T GetStatistics();
        void ResetStatistics();
    }
}
