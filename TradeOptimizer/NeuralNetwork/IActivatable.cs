using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoTrading.Neural
{
    public interface IActivatable
    {
        double Delta { get; set; }
        double WeightedInputSum { get; set; }
        double InnerActivationDerivative { get; set; }
        NeuralSettings.ActivationType Activation { get; set; }
    }
}
