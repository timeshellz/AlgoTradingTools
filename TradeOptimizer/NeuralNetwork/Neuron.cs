using System.Linq;

namespace AlgoTrading.Neural
{
    public abstract class Neuron : Node, ILinearActivatable, IGeluActivatable
    {
        public double Delta { get; set; }
        public double WeightedInputSum { get; set; }
        public double InnerActivationDerivative { get; set; }
        public NeuralConfiguration.ActivationType Activation { get; set; }

        public Neuron(int layer, NeuralConfiguration.ActivationType activationType) : base(layer)
        {
            Layer = layer;
            Activation = activationType;
        }

        public double GetWeightedInputSum()
        {
            double weightedInputSum = 0;

            foreach (NodeConnection connection in Connections.Where(e => e.InputNode != this))
            {
                weightedInputSum += connection.Weight * connection.InputNode.Value;
            }

            return weightedInputSum;
        }

        public void Activate()
        {
            WeightedInputSum = GetWeightedInputSum();

            if (Activation == NeuralConfiguration.ActivationType.Gelu)
            {
                Value = NeuralMath.GELU(WeightedInputSum);
                //InnerActivationDerivative = Differentiate.FirstPartialDerivative(NeuralMath.GELU, new double[] { Value }, 0);
                InnerActivationDerivative = NeuralMath.GELUDerivative(WeightedInputSum);
            }
            else if (Activation == NeuralConfiguration.ActivationType.Linear)
            {
                Value = WeightedInputSum;
                InnerActivationDerivative = 1;
            }
            else if (Activation == NeuralConfiguration.ActivationType.Selu)
            {
                Value = NeuralMath.SELU(WeightedInputSum);
                InnerActivationDerivative = NeuralMath.SELUDerivative(WeightedInputSum);
            }
        }
    }
}
