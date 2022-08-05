﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoTrading.Neural.Persistence.Database.DTO
{
    public class NeuralConfigurationDTO
    {
        public int Id { get; set; }
        public string NetworkName { get; set; }
        public int HiddenLayerCount { get; set; }
    }
}