namespace AlgoTrading.Neural
{
    public interface ISoftMaxActivatable : IActivatable
    {
        void Activate(double maxValue, double exponentSum);
    }
}
