namespace AlgoTrading.Agent.Persistence
{
    public interface IAgentPersistenceManager
    {
        void SaveAgent(IAgent agent);
        IAgent LoadAgent();
    }
}
