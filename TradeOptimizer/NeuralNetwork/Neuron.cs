using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using Newtonsoft.Json;
using MathNet.Numerics.Distributions;

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

            if(Activation == NeuralConfiguration.ActivationType.Gelu)
            {
                Value = NeuralMath.GELU(WeightedInputSum);
                //InnerActivationDerivative = Differentiate.FirstPartialDerivative(NeuralMath.GELU, new double[] { Value }, 0);
                InnerActivationDerivative = NeuralMath.GELUDerivative(WeightedInputSum);
            }
            else if(Activation == NeuralConfiguration.ActivationType.Linear)
            {
                Value = WeightedInputSum;
                InnerActivationDerivative = 1;
            }                       
        }
    }
}
