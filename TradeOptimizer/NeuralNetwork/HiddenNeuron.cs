using System.Linq;

namespace AlgoTrading.Neural
{
    class HiddenNeuron : Neuron
    {
        public HiddenNeuron(int layer) : base(layer, NeuralConfiguration.ActivationType.Selu)
        {

        }

        public double GetDelta()
        {
            Delta = 0;

            foreach (NodeConnection connection in Connections.Where(e => e.InputNode == this))
            {
                Delta += connection.Weight * ((Neuron)connection.OutputNode).Delta;
            }

            return Delta *= InnerActivationDerivative;
        }
    }
}
