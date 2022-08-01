using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using System.Runtime.Serialization;

using AlgoTrading.Utilities;

namespace AlgoTrading.Neural
{
    public class NeuralNetwork
    {
        public string Name { get; private set; }
        public Dictionary<int, List<INode>> Nodes { get; private set; }
        //public Dictionary<string, Dictionary<int, CategoricalOutputNeuron>> CategoricalOutputs { get; private set; }
        public List<NodeConnection> NodeConnections { get; private set; }
        public NeuralConfiguration Settings { get; private set; }

        public NeuralNetwork(string name, NeuralConfiguration settings)
        {
            Name = name;
            Settings = settings;

            Nodes = new Dictionary<int, List<INode>>();
            NodeConnections = new List<NodeConnection>();

            NodeConnection.lastID = 0;
            CreateInputNodes();
            CreateHiddenLayers();
            CreateOutputNeurons();
        }

        [OnDeserialized]
        void ReassignConnections(StreamingContext context)
        {
            foreach(NodeConnection connection in NodeConnections)
            {
                foreach(List<INode> layerNodes in Nodes.Values)
                {
                    foreach (INode node in layerNodes)
                    {
                        if (connection.InputNode == node || connection.OutputNode == node)
                            node.Connections.Add(connection);
                    }
                }
                
            }
        }

        void CreateInputNodes()
        {
            Nodes.Add(0, new List<INode>());

            foreach(string inputName in Settings.Inputs)
            {
                Nodes[0].Add(new InputNode(0, inputName));
            }
        }

        void CreateHiddenLayers()
        {
            for(int i = 0; i < Settings.HiddenLayerCount; i++)
            {
                Nodes.Add(i + 1, new List<INode>());

                for(int j = 0; j < Settings.Inputs.Count; j++)
                {
                    HiddenNeuron newNeuron = new HiddenNeuron(i + 1);
                    Nodes[i + 1].Add(newNeuron);

                    GenerateNewNeuralConnections(newNeuron);
                }
            }
        }

        void CreateOutputNeurons()
        {
            Nodes.Add(Settings.HiddenLayerCount + 1, new List<INode>());

            //if(Settings.PredictionType == NeuralConfiguration.PredictionMechanism.NonCategorical)
            //{
                foreach (string outputName in Settings.Outputs)
                {
                    NonCategoricalOutputNeuron outputNode = new NonCategoricalOutputNeuron(Settings.HiddenLayerCount + 1, outputName);
                    Nodes[outputNode.Layer].Add(outputNode);

                    GenerateNewNeuralConnections(outputNode);
                }
            //}
            //else
            //{
            //    Nodes.Add(Settings.HiddenLayerCount + 2, new List<INode>());

            //    CategoricalOutputs = new Dictionary<string, Dictionary<int, CategoricalOutputNeuron>>();

            //    foreach (string outputName in Settings.Outputs)
            //    {
            //        CategoricalOutputs.Add(outputName, new Dictionary<int, CategoricalOutputNeuron>());

            //        for(int i = 0; i < Settings.AtomCount; i++)
            //        {
            //            CategoricalOutputNeuron outputNode = new CategoricalOutputNeuron(Settings.HiddenLayerCount + 1, outputName);
            //            Nodes[outputNode.Layer].Add(outputNode);

            //            outputNode.AtomSupport = Settings.MinV + i * Settings.DeltaZ;

            //            GenerateNewNeuralConnections(outputNode);

            //            CategoricalOutputs[outputName].Add(i, outputNode);
            //        }

            //        foreach(CategoricalOutputNeuron outputNeuron in CategoricalOutputs[outputName].Values)
            //        {
            //            SoftMaxNode softMaxNode = new SoftMaxNode(outputNeuron);
            //            Nodes[softMaxNode.Layer].Add(softMaxNode);

            //            GenerateNewNeuralConnections(softMaxNode);
            //        }
            //    }
            //}
        }

        void GenerateNewNeuralConnections(INode newNode)
        {            
            if (newNode.Layer < 1)
                throw new ArgumentException();

            double randomWeight;

            foreach (INode node in Nodes[newNode.Layer - 1])
            {
                randomWeight = NeuralMath.He(RandomGenerator.Generate(-1000000, 1000000)/1000000, Nodes[newNode.Layer - 1].Count);
                NodeConnection newConnection = newNode.Connect(node, ConnectionDirection.LeftToRight, randomWeight);

                NodeConnections.Add(newConnection);
            }
        }

        public void FillInputs(Dictionary<string, double> newInputs, bool standardize = true)
        {
            if (newInputs.Values.Count != Settings.Inputs.Count)
                throw new ArgumentException("Input count not equal to neural network specifications.");

            if(standardize)
                NeuralMath.StandardizeData(newInputs);

            foreach(INode node in Nodes[0])
            {
                InputNode ioNode = (InputNode)node;
                ioNode.Value = newInputs[ioNode.Name];
            }
        }

        public Dictionary<string, double> GetOutputs()
        {
            Dictionary<string, double> output = new Dictionary<string, double>();

            //if(Settings.PredictionType == NeuralConfiguration.PredictionMechanism.NonCategorical)
            //{
                foreach (INode node in Nodes[Nodes.Count - 1])
                {
                    OutputNeuron outputNeuron = (OutputNeuron)node;

                    output.Add(outputNeuron.Name, outputNeuron.Value);
                }
            //}
            //else
            //{
            //    foreach (KeyValuePair<string, Dictionary<int, CategoricalOutputNeuron>> atomSet in CategoricalOutputs)
            //    {
            //        double distributionSum = 0;

            //        foreach(CategoricalOutputNeuron neuron in atomSet.Value.Values)
            //        {
            //            distributionSum += neuron.RelatedSoftMaxOutput.Value * neuron.AtomSupport;
            //        }

            //        output.Add(atomSet.Key, distributionSum);
            //    }
            //}

            return output;
        }

        public void ForwardFeed()
        {
            for(int i = 1; i < Settings.HiddenLayerCount + 2; i++)
            {
                foreach(INode node in Nodes[i])
                {
                    Neuron currentNode = (Neuron)node;
                    currentNode.Activate();
                }
            }

            //if (Settings.PredictionType != NeuralConfiguration.PredictionMechanism.NonCategorical)
            //{
            //    foreach(Dictionary<int, CategoricalOutputNeuron> atomSet in CategoricalOutputs.Values)
            //    {
            //        double maxValue = 0;
            //        double exponentSum = 0;

            //        foreach(CategoricalOutputNeuron outputNeuron in atomSet.Values)
            //        {
            //            if (outputNeuron.Value > maxValue)
            //                maxValue = outputNeuron.Value;
            //        }

            //        foreach (CategoricalOutputNeuron outputNeuron in atomSet.Values)
            //        {
            //            exponentSum += Math.Exp(outputNeuron.Value - maxValue);
            //        }

            //        foreach(CategoricalOutputNeuron outputNeuron in atomSet.Values)
            //        {
            //            outputNeuron.RelatedSoftMaxOutput.Activate(maxValue, exponentSum);
            //        }
            //    }
            //}           
        }

        public void Backpropagate(IBackpropagationSpecification specification)
        {
            PropagateOutputs(specification);

            for(int i = Settings.HiddenLayerCount; i >= 1; i--)
            {
                foreach(INode node in Nodes[i])
                {
                    HiddenNeuron neuron = (HiddenNeuron)node;
                    neuron.GetDelta();

                    foreach (NodeConnection connection in node.Connections.Where(e => e.OutputNode == node))
                    {
                        connection.CalculateGradient();
                    }
                }            
            }

            foreach (INode node in Nodes[Settings.HiddenLayerCount + 1])
            {
                foreach (NodeConnection connection in node.Connections.Where(e => e.OutputNode == node))
                {
                    connection.CalculateGradient();
                }
            }
        }       

        void PropagateOutputs(IBackpropagationSpecification specification)
        {
            //if(specification.PredictionType == NeuralConfiguration.PredictionMechanism.NonCategorical)
            //{
                foreach(INode node in Nodes[Nodes.Keys.Count - 1])
                {
                    if (node.GetType() == typeof(NonCategoricalOutputNeuron))
                    {
                        NonCategoricalOutputNeuron outputNeuron = (NonCategoricalOutputNeuron)node;                       
                        outputNeuron.GetDelta((HuberLossBackpropagationSpecification)specification);
                    }
                }
            //}
            //else
            //{
            //    if(specification is CrossEntropyBackpropagationSpecification)
            //    {
            //        CrossEntropyBackpropagationSpecification spec = (CrossEntropyBackpropagationSpecification)specification;
            //        Dictionary<int, double> targetProbabilities = new Dictionary<int, double>();

            //        foreach (KeyValuePair<int, CategoricalOutputNeuron> outputAtom in CategoricalOutputs[specification.TargetName])
            //        {
            //            targetProbabilities.Add(outputAtom.Key, 0);
            //        }

            //        foreach (KeyValuePair<int, CategoricalOutputNeuron> outputAtom in CategoricalOutputs[specification.TargetName])
            //        {
            //            double b = (spec.TargetValues[outputAtom.Key] - Settings.MinV) / Settings.DeltaZ;
            //            double l = Math.Floor(b);
            //            double u = Math.Ceiling(b);

            //            targetProbabilities[(int)l] += spec.EstimatedTargetValueProbabilities[outputAtom.Key] * (u - b);
            //            targetProbabilities[(int)u] += spec.EstimatedTargetValueProbabilities[outputAtom.Key] * (b - l);
            //        }

            //        foreach (KeyValuePair<int, CategoricalOutputNeuron> outputAtom in CategoricalOutputs[specification.TargetName])
            //        {
            //            outputAtom.Value.GetDelta(spec, targetProbabilities[outputAtom.Key]);
            //        }
            //    }
            //    else
            //    {
            //       // QuantileBackpropagationSpecification spec = (QuantileBackpropagationSpecification)specification;

            //       // foreach (KeyValuePair<int, OutputNeuron> outputAtom in CategoricalOutputs[specification.TargetName])
            //        //{
            //        //    outputAtom.Value.GetDelta(spec);
            //        //}
            //    }
            //}
        }

        public void UpdateWeights(bool isAscent = false)
        {
            for(int i = 0; i < Nodes.Keys.Count - 2; i++)
            {
                foreach(INode node in Nodes[i])
                {
                    foreach(NodeConnection connection in node.Connections.Where(e => e.InputNode == node))
                    {
                        connection.UpdateWeight(Settings.LearningRate, isAscent);
                    }
                }
            }
        }

        public Dictionary<int, double> CopyWeights()
        {
            Dictionary<int, double> output = new Dictionary<int, double>();

            for(int i = 0; i < Nodes.Count - 1; i++)
            {
                foreach(INode node in Nodes[i])
                {
                    foreach(NodeConnection connection in node.Connections)
                    {
                        if(!output.ContainsKey(connection.ID))
                            output.Add(connection.ID, connection.Weight);
                    }
                }
            }

            return output;
        }

        public void PasteWeights(Dictionary<int, double> weights)
        {
            for (int i = 0; i < Nodes.Count - 1; i++)
            {
                foreach (INode node in Nodes[i])
                {
                    foreach (NodeConnection connection in node.Connections)
                    {
                        connection.SetWeight(weights[connection.ID]);
                    }
                }
            }
        }

        public Dictionary<int, double> CopyGradients()
        {
            Dictionary<int, double> output = new Dictionary<int, double>();

            for (int i = 0; i < Nodes.Count - 1; i++)
            {
                foreach (INode node in Nodes[i])
                {
                    foreach (NodeConnection connection in node.Connections)
                    {
                        if(!output.ContainsKey(connection.ID))
                            output.Add(connection.ID, connection.Gradient);
                    }
                }
            }

            return output;
        }

        public void PasteGradients(Dictionary<int, double> gradients)
        {
            for (int i = 0; i < Nodes.Count - 1; i++)
            {
                foreach (INode node in Nodes[i])
                {
                    foreach (NodeConnection connection in node.Connections)
                    {
                        connection.SetGradient(gradients[connection.ID]);
                    }
                }
            }
        }
    }

    public class NeuralConfiguration
    {
        public string NetworkName { get; set; } = "DefaultNetwork";
        public List<string> Inputs { get; set; } = new List<string>();
        public List<string> Outputs { get; set; } = new List<string>();
        public int HiddenLayerCount { get; set; } = 3;
        public double LearningRate { get; set; } = 0.01;
        public PredictionMechanism PredictionType { get; set; } = PredictionMechanism.NonCategorical;
        public int AtomCount { get; set; } = 1;
        public double MinV { get; set; } = -10;
        public double MaxV { get; set; } = 10;
        public double DeltaZ { get; set; } = 0;
        
        //public NeuralSettings(string networkName, List<string> inputs, List<string> outputs, int hiddenLayers, double learningRate)
        //{
        //    NetworkName = networkName.Replace(" ", "_");
        //    NetworkName = NetworkName.Replace(".", "_");
        //    Inputs = inputs;
        //    Outputs = outputs;
        //    HiddenLayerCount = hiddenLayers;
        //    LearningRate = learningRate;
        //}

        public NeuralConfiguration() { }

        //public NeuralSettings(string networkName, List<string> inputs, List<string> outputs, int hiddenLayers, double learningRate, PredictionMechanism predictionType, int atomCount) : this(networkName, inputs, outputs, hiddenLayers, learningRate)
        //{            
        //    PredictionType = predictionType;
        //    AtomCount = atomCount;

        //    if(PredictionType == PredictionMechanism.CategoricalCrossEntropy)
        //    {
        //        DeltaZ = (double)(MaxV - MinV) / (AtomCount - 1);
        //    }
        //    else
        //    {
        //        DeltaZ = 1d / AtomCount;
        //        MinV = DeltaZ;
        //        MaxV = 1;
        //    }
            
        //}

        public enum PredictionMechanism { NonCategorical, CategoricalCrossEntropy, CategoricalQuantile}
        public enum ActivationType { Gelu, SoftMax, Linear}
    }
}
