using System;
using System.Threading.Tasks;

namespace AlgoTrading.Agent
{
    public interface IAgent
    {
         Task<bool> Interact();
    }
}
