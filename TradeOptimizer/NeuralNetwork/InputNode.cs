using Newtonsoft.Json;
using System.Collections.Generic;

namespace AlgoTrading.Neural
{
    public class InputNode : INode
    {
        public double Value { get; set; }
        public string Name { get; set; }
        public int Layer { get; set; }
        [JsonIgnore]
        public List<NodeConnection> Connections { get; set; } = new List<NodeConnection>();

        public InputNode(int layer, string name)
        {
            Name = name;
            Layer = layer;
        }

        public InputNode(int layer, string name, List<NodeConnection> connections) : this(layer, name)
        {
            Name = name;
            Layer = layer;
            Connections = connections;
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
