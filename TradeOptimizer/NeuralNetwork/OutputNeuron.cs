using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics;

namespace AlgoTrading.Neural
{
    public abstract class OutputNeuron : Neuron
    {
        public string Name { get; set; }
        //public double AtomSupport { get; set; } = 0;

        public OutputNeuron(int layer, string name, NeuralConfiguration.ActivationType activationType) : base(layer, activationType)
        {
            Name = name;
        }
        
    }
}
