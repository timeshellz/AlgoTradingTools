using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoTrading.Neural.Persistence.Database.DTO
{
    public class NeuronDTO : NodeDTO
    {
        public NeuralConfiguration.ActivationType ActivationType { get; set; }
    }
}
