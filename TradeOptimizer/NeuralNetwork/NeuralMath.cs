using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MathNet.Numerics.Differentiation;


namespace AlgoTrading.Neural
{
    public static class NeuralMath
    {
        public enum LossType { Huber, Actor}

        public static double GELU(double z)
        {
            return 0.5d * z * (1 + Math.Tanh(Math.Sqrt(2d / Math.PI) * (z + 0.0441715d * Math.Pow(z, 3))));
        }

        public static double GELU(double[] z)
        {
            return 0.5d * z[0] * (1 + Math.Tanh(Math.Sqrt(2d / Math.PI) * (z[0] + 0.0441715d * Math.Pow(z[0], 3))));
        }

        public static double GELUDerivative(double z)
        {
            return 0.5d * Math.Tanh(0.0356774 * Math.Pow(z, 3) + 0.797885 * z) + (0.0535161 * Math.Pow(z, 3) + 0.398942 * z) *
                Math.Pow(1d / Math.Cosh(0.0356774 * Math.Pow(z, 3) + 0.797885 * z), 2) + 0.5d;
        }

        public static double HuberLoss(double temporalDifference)
        {
            return Math.Sqrt(1 + Math.Pow(temporalDifference, 2)) - 1;
        }

        public static double HuberLossDerivative(double temporalDifference)
        {
            double a = temporalDifference;
            return a / Math.Sqrt(Math.Pow(a, 2) + 1);
        }

        public static double SoftMax(double value, double maxValue, double exponentSum)
        {
            double test = Math.Exp(value - maxValue) / exponentSum;

            return test;
        }

        public static double SoftMaxDerivative(double value, double maxValue, double exponentSum)
        {
            Func<double, double> softMax = x => SoftMax(x, maxValue, exponentSum);
            var deriv = new NumericalDerivative(3, 1);
            double test1 = deriv.EvaluateDerivative(softMax, value, 1);

            //double yn = SoftMax(value, maxValue, exponentSum);       

            return test1;//yn * (1 - yn);
        }

        public static double CrossEntropyLoss(double estimatedProbability, double targetProbability)
        {
            return -1 * targetProbability * Math.Log(Math.Max(0.00000000000001, estimatedProbability));
        }

        public static double CrossEntropyDerivative(double targetProbability, double estimatedProbability)
        {
             //Func<double, double> crossEntropy = x => CrossEntropyLoss(x, targetProbability);

            //var deriv = new NumericalDerivative(3, 1);
            //double test = deriv.EvaluateDerivative(crossEntropy, estimatedProbability, 1);
            return -1 * targetProbability / estimatedProbability; //- Math.Log(estimatedValue) * HuberLossDerivative(criticTarget - estimatedValue);
        }

        public static Dictionary<int, double> GetSoftMaxDistribution(Dictionary<int, double> inputValues, out double maxValue, out double probabilitySum)
        {
            Dictionary<int, double> outputDistribution = new Dictionary<int, double>();

            probabilitySum = 0;
            maxValue = double.MinValue;

            foreach (double value in inputValues.Values)
            {
                if (value > maxValue)
                    maxValue = value;
            }

            foreach (double value in inputValues.Values)
            {
                probabilitySum += Math.Exp(value - maxValue);
            }

            for (int i = 0; i < inputValues.Count; i++)
            {
                outputDistribution.Add(i, SoftMax(inputValues[i], maxValue, probabilitySum));
            }

            return outputDistribution;
        }

        public static double Z(double w, double a, double b)
        {
            return w * a + b;
        }

        public static double Z(double[] values)
        {
            return values[0] * values[1] + values[2];
        }

        public static double Q(double reward, double discount, double nextQ = 0)
        {
            return reward + discount * nextQ;
        }

        public static double He(double weight, int dimesionOfPreviousLayer)
        {
            return weight * Math.Sqrt(2d / dimesionOfPreviousLayer);
        }

        public static Dictionary<string, double> GetStandardizedData(Dictionary<string, double> input)
        {
            var output = new Dictionary<string, double>(input);

            double mean = 0;
            double variance = 0;

            int count = output.Values.Count;

            foreach (double value in output.Values)
            {
                mean += value;
            }

            mean /= count;

            foreach (double value in output.Values)
            {
                variance += Math.Pow(value - mean, 2);
            }

            variance = Math.Sqrt(variance/count);

            for (int i = 0; i < output.Values.Count; i++)
            {
                var element = output.ElementAt(i);
                output.Remove(element.Key);
                output.Add(element.Key, (element.Value - mean) / variance);
            }

            return output;
        }
    }
}
