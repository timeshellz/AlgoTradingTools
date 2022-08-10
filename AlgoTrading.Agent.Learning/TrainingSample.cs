using System.Collections.Generic;

namespace AlgoTrading.Agent.Learning
{
    public struct TrainingSample
    {
        public Dictionary<string, double> State { get; private set; }
        public string Action { get; private set; }
        public Dictionary<int, double> Targets { get; private set; }

        public TrainingSample(Dictionary<int, double> targets, string action, Dictionary<string, double> state)
        {
            State = state;
            Targets = targets;
            Action = action;
        }

        public TrainingSample(Dictionary<int, double> targets, Dictionary<int, double> targetProbabilities, string action,
            Dictionary<string, double> state)
        {
            State = state;
            Targets = targets;
            Action = action;
        }
    }
}
