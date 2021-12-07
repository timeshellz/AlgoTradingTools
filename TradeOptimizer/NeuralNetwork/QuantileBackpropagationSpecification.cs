using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoTrading.Neural
{
    public class QuantileBackpropagationSpecification : IBackpropagationSpecification
    {
        public string TargetName { get; set; }
        public double TemporalDifference { get; set; }
        public NeuralSettings.PredictionMechanism PredictionType { get; set; }

        public QuantileBackpropagationSpecification(string targetName, double temporalDifference)
        {
            PredictionType = NeuralSettings.PredictionMechanism.CategoricalQuantile;
            TargetName = targetName;
            TemporalDifference = temporalDifference;
        }

        public double CalculateLossDerivative(double quantileSupport, double predictedProbability, double weightedInputSum)
        {
            //return (TemporalDifference * Math.Abs(quantileSupport - predictedProbability)) / Math.Sqrt(Math.Pow(TemporalDifference, 2) + 1);
            return quantileSupport * weightedInputSum + (quantileSupport - predictedProbability) * weightedInputSum;
        }
    }
}
