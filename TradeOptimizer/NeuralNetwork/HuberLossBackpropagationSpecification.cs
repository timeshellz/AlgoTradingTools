using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoTrading.Neural
{
    public class HuberLossBackpropagationSpecification : IBackpropagationSpecification
    {
        public NeuralConfiguration.PredictionMechanism PredictionType { get; set; } = NeuralConfiguration.PredictionMechanism.NonCategorical;
        public string TargetName { get; set; }
        public double TargetValue { get; private set; }

        public HuberLossBackpropagationSpecification(string targetName, double targetValue)
        {
            TargetValue = targetValue;
            TargetName = targetName;
        }

        public double CalculateLossDerivative(double observedValue)
        {
            return NeuralMath.HuberLossDerivative(observedValue - TargetValue);
        }
    }
}
