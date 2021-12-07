using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoTrading.Neural
{
    public interface INode
    {
        List<NodeConnection> Connections { get; set; }
        int Layer { get; set; }
        double Value { get; set; }

        NodeConnection Connect(INode otherNode, ConnectionDirection direction, double weight);
    }
}
