namespace AlgoTrading.DQN.Statistics
{
    public struct IterationStatistics
    {
        public double TotalEstimatedQ { get; private set; }
        public double TotalReward { get; private set; }
        public double TotalLoss { get; private set; }
        public int EstimationCount { get; private set; }
        public int CollectedMemoriesCount { get; private set; }
        public bool IsEpochOver { get; private set; }

        public int TradeCount { get; private set; }
        public double TotalCommisionedTradeProfit { get; private set; }
        public double TotalTradeDuration { get; private set; }

        public IterationStatistics(double totalEstimatedQ, double totalIterationReward, double totalLoss,
            int estimationCount, int memoriesCount, bool isOver, int tradeCount, double totalTradeProfit, double totalTradeDuration)
        {
            TotalEstimatedQ = totalEstimatedQ;
            TotalReward = totalIterationReward;
            TotalLoss = totalLoss;
            EstimationCount = estimationCount;
            CollectedMemoriesCount = memoriesCount;
            IsEpochOver = isOver;

            TradeCount = tradeCount;
            TotalCommisionedTradeProfit = totalTradeProfit;
            TotalTradeDuration = totalTradeDuration;
        }
    }
}
