using System.Collections.Generic;
using System.Linq;

namespace AlgoTrading.Neural.Persistence.Database.DTO
{
    public static class NeuralModelExtensions
    {
        public static NeuralNetworkDTO GetDTO(this NeuralNetwork network)
        {
            if (network == null)
                return null;

            var connections = network.NodeConnections.ToDictionary(k => k.ID);
            var connectionDTOs = new Dictionary<int, NodeConnectionDTO>();

            var nodeList = network.Nodes.SelectMany(kv => kv.Value).ToList();
            var nodeDTOList = nodeList.Select(n => n.GetDTO()).ToList();

            for (int i = 0; i < nodeList.Count; i++)
            {
                foreach (var connectionId in nodeList[i].Connections.Select(c => c.ID))
                {
                    if (!connectionDTOs.ContainsKey(connectionId))
                        connectionDTOs.Add(connectionId, connections[connectionId].GetDTO());

                    if (connections[connectionId].InputNode == nodeList[i])
                        connectionDTOs[connectionId].InputNode = nodeDTOList[i];

                    if (connections[connectionId].OutputNode == nodeList[i])
                        connectionDTOs[connectionId].OutputNode = nodeDTOList[i];
                }
            }

            return new NeuralNetworkDTO()
            {
                Configuration = network.Settings.GetDTO(),
                Inputs = nodeDTOList.Where(n => n is InputNodeDTO).Cast<InputNodeDTO>().ToList(),
                Neurons = nodeDTOList.Where(n => n is NeuronDTO && !(n is OutputNeuronDTO)).Cast<NeuronDTO>().ToList(),
                Outputs = nodeDTOList.Where(n => n is OutputNeuronDTO).Cast<OutputNeuronDTO>().ToList(),
                Connections = connectionDTOs.Select(kv => kv.Value).ToList(),
            };
        }

        public static NeuralNetworkDTO PasteConnectionWeights(this NeuralNetworkDTO neuralNetworkDTO1, List<NodeConnectionDTO> connectionDTOs2)
        {
            for (int i = 0; i < neuralNetworkDTO1.Connections.Count; i++)
            {
                neuralNetworkDTO1.Connections[i].Weight = connectionDTOs2[i].Weight;
            }

            return neuralNetworkDTO1;
        }

        public static NeuralNetwork GetModel(this NeuralNetworkDTO networkDTO)
        {
            if (networkDTO == null)
                return null;

            return new NeuralNetwork(networkDTO.Inputs.Select(d => d.GetModel()).ToList(), networkDTO.Neurons.Select(d => d.GetModel()).ToList(),
                networkDTO.Outputs.Select(d => d.GetModel()).ToList(), networkDTO.Configuration.GetModel());
        }

        public static NodeDTO GetDTO(this INode node)
        {
            if (node == null)
                return null;

            if (node is InputNode inputNode)
            {
                return new InputNodeDTO()
                {
                    Name = inputNode.Name,
                    Layer = inputNode.Layer,
                };
            }
            else if (node is OutputNeuron outputNeuron)
            {
                return new OutputNeuronDTO()
                {
                    Name = outputNeuron.Name,
                    Layer = outputNeuron.Layer,
                };
            }
            else if (node is Neuron neuron)
            {
                return new NeuronDTO()
                {
                    Layer = neuron.Layer,
                    ActivationType = neuron.Activation,
                };
            }

            return null;
        }

        public static INode GetModel(this NodeDTO nodeDTO)
        {
            INode node = null;

            if (node == null)
                return null;

            if (nodeDTO is InputNodeDTO inputNodeDTO)
            {
                var inputNode = (InputNode)node;

                inputNode.Layer = inputNodeDTO.Layer;
                inputNode.Name = inputNodeDTO.Name;

                return inputNode;
            }
            else if (nodeDTO is OutputNeuronDTO outputNeuronDTO)
            {
                var outputNeuron = (OutputNeuron)node;

                outputNeuron.Name = outputNeuronDTO.Name;
                outputNeuron.Layer = outputNeuron.Layer;

                return outputNeuron;
            }
            else if (nodeDTO is NeuronDTO neuronDTO)
            {
                var neuron = (Neuron)node;

                neuron.Activation = neuronDTO.ActivationType;
                neuron.Layer = neuronDTO.Layer;

                return neuron;
            }

            return node;
        }

        public static NodeConnectionDTO GetDTO(this NodeConnection connection)
        {
            if (connection == null)
                return null;

            return new NodeConnectionDTO()
            {
                LocalId = connection.ID,
                Weight = connection.Weight,
            };
        }

        public static NodeConnection GetModel(this NodeConnectionDTO connectionDTO)
        {
            if (connectionDTO == null)
                return null;

            INode inputNode = connectionDTO.InputNode.GetModel();
            INode outputNode = connectionDTO.OutputNode.GetModel();

            return new NodeConnection(inputNode, outputNode, connectionDTO.Weight);
        }

        public static NeuralConfigurationDTO GetDTO(this NeuralConfiguration configuration)
        {
            if (configuration == null)
                return null;

            return new NeuralConfigurationDTO()
            {
                HiddenLayerCount = configuration.HiddenLayerCount,
                NetworkName = configuration.NetworkName,
            };
        }

        public static NeuralConfiguration GetModel(this NeuralConfigurationDTO configurationDTO)
        {
            if (configurationDTO == null)
                return null;

            return new NeuralConfiguration()
            {
                HiddenLayerCount = configurationDTO.HiddenLayerCount,
                NetworkName = configurationDTO.NetworkName,
            };
        }
    }
}
