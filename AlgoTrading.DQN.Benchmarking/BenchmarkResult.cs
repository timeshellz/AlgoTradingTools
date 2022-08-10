namespace AlgoTrading.DQN.Benchmarking
{
    class BenchmarkResult
    {
        public BenchmarkCategory Category { get; set; }
        public double AverageAbsoluteStockPeriodPriceChange { get; set; }
        public double AverageSkilledProfitPercentage { get; set; }
        public double AverageProfitFactor { get; set; }
        public double AveragePercentProfitable { get; set; }
    }
}
