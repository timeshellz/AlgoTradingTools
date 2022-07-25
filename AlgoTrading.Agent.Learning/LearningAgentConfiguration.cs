using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoTrading.Agent.Learning
{
    public class LearningAgentConfiguration : AgentConfiguration
    {       
        public int MemoryBuferSize { get; set; } = 500000;
        public int BatchSize { get; set; } = 32;
        public double Discount { get; set; } = 0.99;

        private double initialEpsilon = 0.99;
        public double InitialEpsilon
        {
            get => initialEpsilon;
            set
            {
                initialEpsilon = value;
                CurrentEpsilon = value;
            }
        }
        public double CurrentEpsilon { get; set; } = 0.99;
        public double MinimumEpsilon { get; set; } = 0.1;
        public double EpsilonDecay { get; set; } = 0.99;
        public int MemorySteps { get; set; } = 10;
        public double Tau { get; set; } = 0.01;

        public LearningAgentConfiguration() { }
    }
}
