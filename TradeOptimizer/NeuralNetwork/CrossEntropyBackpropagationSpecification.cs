using System.Collections.Generic;

namespace AlgoTrading.Neural
{
    public class CrossEntropyBackpropagationSpecification : IBackpropagationSpecification
    {
        public NeuralConfiguration.PredictionMechanism PredictionType { get; set; } = NeuralConfiguration.PredictionMechanism.CategoricalCrossEntropy;
        public string TargetName { get; set; }
        public Dictionary<int, double> TargetValues { get; set; }
        public Dictionary<int, double> EstimatedTargetValueProbabilities { get; set; }
        public double Discount { get; set; }

        public CrossEntropyBackpropagationSpecification(string targetName, Dictionary<int, double> targetValues, Dictionary<int, double> targetProbabilities)
        {
            TargetName = targetName;
            TargetValues = targetValues;
            EstimatedTargetValueProbabilities = targetProbabilities;
        }

        public double CalculateLossDerivative(double targetProbability, double estimatedProbability)
        {
            double test = NeuralMath.CrossEntropyDerivative(targetProbability, estimatedProbability);

            if (double.IsNaN(test))
            {
                int s = 0;
            }

            return test;
        }
    }
}
