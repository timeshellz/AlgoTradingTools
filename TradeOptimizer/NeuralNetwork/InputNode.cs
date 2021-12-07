using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using MathNet.Numerics.Distributions;

namespace AlgoTrading.Neural
{
    class InputNode : INode
    {
        public double Value { get; set; }
        public string Name { get; set; }
        public int Layer { get; set; }
        [JsonIgnore]
        public List<NodeConnection> Connections { get; set; }

        public InputNode(int layer, string name)
        {
            Name = name;
            Layer = layer;
            Connections = new List<NodeConnection>();
        }


        public NodeConnection Connect(INode otherNode, ConnectionDirection direction, double weight)
        {
            NodeConnection connection;

            if (direction == ConnectionDirection.LeftToRight)
                connection = new NodeConnection(otherNode, this, weight);
            else
                connection = new NodeConnection(this, otherNode, weight);

            Connections.Add(connection);
            otherNode.Connections.Add(connection);

            return connection;
        }
    }
}
