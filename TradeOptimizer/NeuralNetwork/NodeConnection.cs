using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MathNet.Numerics;
using Newtonsoft.Json;

namespace AlgoTrading.Neural
{
    public class NodeConnection
    {
        public static int lastID = 0;
        public int ID { get; private set; }
        public double Weight { get; private set; }
        public double Gradient { get; private set; } = 0;
        public INode InputNode { get; private set; }
        public INode OutputNode { get; private set; }

        public NodeConnection(INode inputNode, INode outputNode, double weight)
        {
            ID = lastID;
            lastID++;
            Weight = weight;
            InputNode = inputNode;
            OutputNode = outputNode;
        }

        public double CalculateGradient()
        {
            return Gradient = ((Neuron)OutputNode).Delta * InputNode.Value;
        }


        public void SetGradient(double newValue)
        {
            Gradient = newValue;
        }

        public void UpdateWeight(double learningRate, bool isAscent = false)
        {
            if (!isAscent)
                Weight -= learningRate * Gradient;
            else
                Weight += learningRate * Gradient;
        }

        public void SetWeight(double weight)
        {
            Weight = weight;
        }
    }

    public enum ConnectionDirection { LeftToRight, RightToLeft }
}
