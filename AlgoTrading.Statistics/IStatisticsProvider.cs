namespace AlgoTrading.Statistics
{
    public interface IStatisticsProvider<T> where T : IStatistics
    {
        T GetStatistics();
        void ResetStatistics();
    }
}
