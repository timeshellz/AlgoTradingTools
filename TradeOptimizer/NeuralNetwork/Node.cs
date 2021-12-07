using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AlgoTrading.Neural
{
    public class Node : INode
    {
        public List<NodeConnection> Connections { get; set; }
        public int Layer { get; set; }
        public double Value { get; set; }

        public Node(int layer)
        {
            Connections = new List<NodeConnection>();
            Layer = layer;
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
