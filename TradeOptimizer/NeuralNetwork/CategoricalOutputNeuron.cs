using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AlgoTrading.Neural
{
    public class CategoricalOutputNeuron : OutputNeuron
    {
        public SoftMaxNode RelatedSoftMaxOutput { get; set; }

        public CategoricalOutputNeuron(int layer, string name) : base(layer, name, NeuralSettings.ActivationType.Linear)
        {
        }

        public double GetDelta(CrossEntropyBackpropagationSpecification specification, double targetProbability)
        {
            Delta = 0;

            if (specification.TargetName == Name)
            {
                foreach (NodeConnection connection in Connections.Where(e => e.InputNode == this))
                {
                    SoftMaxNode outputNode = (SoftMaxNode)connection.OutputNode;
                    outputNode.GetDelta(specification, targetProbability);

                    double outputToInputDerivative = 0;

                    if (outputNode.RelatedNeuron == this)
                        outputToInputDerivative = outputNode.Value * (1 - outputNode.Value);
                    else
                        outputToInputDerivative = -1 * outputNode.Value * RelatedSoftMaxOutput.Value;

                    Delta += outputNode.Delta * outputToInputDerivative;
                }
            }
            else
                Delta = 0;

            return Delta;
        }

       /* public double GetDelta(QuantileBackpropagationSpecification specification)
        {
            if (specification.TargetName == Name)
            {
                InnerActivationDerivative = 1;
                Delta = specification.CalculateLossDerivative(AtomSupport, SoftProbability, WeightedInputSum);
            }
            else
                Delta = 0;

            return Delta;
        }*/
    }
}
