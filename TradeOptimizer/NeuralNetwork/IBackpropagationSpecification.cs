﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoTrading.Neural
{
    public interface IBackpropagationSpecification
    {
        string TargetName { get; set; }
        NeuralConfiguration.PredictionMechanism PredictionType { get; set; }
    }
}
