namespace AlgoTrading.Neural
{
    public class QuantileBackpropagationSpecification : IBackpropagationSpecification
    {
        public string TargetName { get; set; }
        public double TemporalDifference { get; set; }
        public NeuralConfiguration.PredictionMechanism PredictionType { get; set; }

        public QuantileBackpropagationSpecification(string targetName, double temporalDifference)
        {
            PredictionType = NeuralConfiguration.PredictionMechanism.CategoricalQuantile;
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
