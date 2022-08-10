using System.Collections.Generic;

namespace AlgoTrading.Neural.Persistence.Database.DTO
{
    public class NeuralNetworkDTO
    {
        public int Id { get; set; }
        public List<InputNodeDTO> Inputs { get; set; }
        public List<NeuronDTO> Neurons { get; set; }
        public List<OutputNeuronDTO> Outputs { get; set; }
        public List<NodeConnectionDTO> Connections { get; set; }
        public NeuralConfigurationDTO Configuration { get; set; }
    }
}
