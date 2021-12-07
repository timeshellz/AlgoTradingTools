using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoTrading.Neural
{
    public class NeuralMemory
    {
        public int TimeStep { get; private set; }
        public Dictionary<int, Dictionary<string, double>> States { get; private set; }
        public Dictionary<int, string> Actions { get; private set; }
        public Dictionary<int, List<string>> PossibleActions { get; private set; }
        public Dictionary<int, double> Rewards { get; private set; }
        public bool Complete { get; private set; } = false;
        public bool EpisodeEnded { get; private set; } = false;
        public double AbsoluteTemporalDifference { get; private set; } = 0;

        public NeuralMemory(Dictionary<string, double> state, List<string> possibleActions)
        {
            TimeStep = 0;

            States = new Dictionary<int, Dictionary<string, double>>();
            Actions = new Dictionary<int, string>();
            PossibleActions = new Dictionary<int, List<string>>();
            Rewards = new Dictionary<int, double>();

            States.Add(TimeStep, state);
            PossibleActions.Add(TimeStep, possibleActions);
        }

        public void PerformTransition(double reward, Dictionary<string, double> newState, List<string> newPossibleActions)
        {
            Rewards.Add(TimeStep, reward);

            TimeStep++;
            States.Add(TimeStep, newState);
            PossibleActions.Add(TimeStep, newPossibleActions);
        }

        public void ExecuteAction(string action)
        {
            Actions.Add(TimeStep, action);
        }

        public void SetTemporalDifference(double newTemporalDifference)
        {
            AbsoluteTemporalDifference = Math.Abs(newTemporalDifference);
        }

        public void CompleteMemory()
        {
            Complete = true;
        }

        public void CompleteEpisode()
        {
            EpisodeEnded = true;
        }
    }
}
