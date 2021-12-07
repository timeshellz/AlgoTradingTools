using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AlgoTrading.Neural
{
    public class SoftMaxNode : Node, ISoftMaxActivatable
    {
        public string Name { get; set; }
        public CategoricalOutputNeuron RelatedNeuron { get; set; }
        public double Delta { get; set; }
        public double WeightedInputSum { get; set; }
        public double InnerActivationDerivative { get; set; }
        public NeuralSettings.ActivationType Activation { get; set; }

        public SoftMaxNode(CategoricalOutputNeuron relatedNeuron) : base(relatedNeuron.Layer + 1)
        {
            Name = relatedNeuron.Name;
            RelatedNeuron = relatedNeuron;
            relatedNeuron.RelatedSoftMaxOutput = this;
            Activation = NeuralSettings.ActivationType.SoftMax;
        }

        public void Activate(double maxValue, double exponentSum)
        {
            Value = NeuralMath.SoftMax(RelatedNeuron.Value, maxValue, exponentSum);
        }

        public double GetDelta(CrossEntropyBackpropagationSpecification specification, double targetProbability)
        {
            //Delta = (Value - expectedValue) * InnerActivationDerivative;

            if (specification.TargetName == Name)
            {
                Delta = specification.CalculateLossDerivative(targetProbability, Value);
            }
            else
                Delta = 0;

            return Delta;
        }
    }
}
