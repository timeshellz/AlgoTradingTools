using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoTrading.Neural
{
    class NonCategoricalOutputNeuron : OutputNeuron
    {
        public NonCategoricalOutputNeuron(int layer, string name) : base(layer, name, NeuralConfiguration.ActivationType.Gelu)
        {
        }

        public double GetDelta(HuberLossBackpropagationSpecification specification)
        {
            if (specification.TargetName == Name)
                Delta = specification.CalculateLossDerivative(Value) * InnerActivationDerivative;
            else
                Delta = 0;

            return Delta;
        }
    }
}
