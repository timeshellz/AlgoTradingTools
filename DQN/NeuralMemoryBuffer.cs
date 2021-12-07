using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlgoTrading.Neural;
using AlgoTrading.Utilities;

namespace AlgoTrading.DQN
{
    public class NeuralMemoryBuffer : ICollection<NeuralMemory>
    {
        public int Count { get; private set; } = 0;       
        public bool IsReadOnly { get; private set; } = false;
        public double PrioritySum { get; private set; } = 0;
        public double MaxPriority { get; private set; } = 1;

        Dictionary<NeuralMemory, double> memoryProbabilityBuffer = new Dictionary<NeuralMemory, double>();
        SortedList<double, NeuralMemory> reverseMemoryProbabilityBuffer = new SortedList<double, NeuralMemory>(new DescendingComparer<double>());

        double probabilityConstant = 0.05d;

        public int Size { get; private set; }

        public NeuralMemoryBuffer(int bufferSize)
        {
            Size = bufferSize;
        }

        public void Add(NeuralMemory item)
        {
            double itemPriority;

            itemPriority = MaxPriority + 0.000000001;
            PrioritySum += itemPriority;

            while(true)
            {
                int attemptsCount = 0;

                if (reverseMemoryProbabilityBuffer.ContainsKey(itemPriority))
                {
                    itemPriority -= RandomGenerator.Generate(0, 1000000000)/1000000000000;

                    if (attemptsCount > 500)
                        throw new ArgumentException();
                }                  
                else
                    break;
            }

            if (itemPriority > MaxPriority)
                MaxPriority = itemPriority;

            Count++;
            memoryProbabilityBuffer.Add(item, itemPriority);
            reverseMemoryProbabilityBuffer.Add(itemPriority, item);

            if (Count > Size)
            {
                Remove(memoryProbabilityBuffer.First().Key);
            }
        }

        public void Add(List<NeuralMemory> items)
        {
            foreach(NeuralMemory item in items)
            {
                Add(item);
            }
        }

        public void UpdatePriority(NeuralMemory item)
        {
            double oldPriority = memoryProbabilityBuffer[item];
            double newPriority = item.AbsoluteTemporalDifference + probabilityConstant;
           
            while (true)
            {
                int attemptsCount = 0;

                if (reverseMemoryProbabilityBuffer.ContainsKey(newPriority))
                {
                    newPriority -= RandomGenerator.Generate(0, 1000000000) / 1000000000000;

                    if (attemptsCount > 500)
                        throw new ArgumentException();
                }
                else
                    break;
            }

            memoryProbabilityBuffer[item] = newPriority;

            reverseMemoryProbabilityBuffer.Remove(oldPriority);
            reverseMemoryProbabilityBuffer.Add(newPriority, item);

            PrioritySum = PrioritySum - oldPriority + newPriority;

            if (newPriority > MaxPriority)
                MaxPriority = newPriority;
        }

        public double GetProbability(NeuralMemory item, int batchSize)
        {
            return memoryProbabilityBuffer[item] / batchSize;
        }

        public List<NeuralMemory> SampleBatch(int batchSize)
        {
            int rangeWidth = (int)(PrioritySum / batchSize);
            int valuesPerRange = (int)Math.Ceiling((double)batchSize * (double)rangeWidth / Count);

            List<NeuralMemory> sampledBatch = new List<NeuralMemory>();

            int i = 0;

            while (i <= PrioritySum - rangeWidth)
            {
                for(int j = 0; j < valuesPerRange; j++)
                {
                    int randomElement = (int)RandomGenerator.Generate(i, i + valuesPerRange);

                    sampledBatch.Add(reverseMemoryProbabilityBuffer.Values[randomElement]);

                    if (sampledBatch.Count == batchSize)
                        return sampledBatch;                   
                }

                i += rangeWidth;
            }

            return sampledBatch;
        }

        public void Clear()
        {
            memoryProbabilityBuffer.Clear();
            reverseMemoryProbabilityBuffer.Clear();
            Count = 0;
            PrioritySum = 0;
            MaxPriority = 0;
        }

        public bool Contains(NeuralMemory item)
        {
            if (memoryProbabilityBuffer.ContainsKey(item))
                return true;

            return false;
        }

        public void CopyTo(NeuralMemory[] array, int arrayIndex)
        {
            List<NeuralMemory> outputList = memoryProbabilityBuffer.Keys.ToList();
            Array.Copy(outputList.ToArray(), 0, array, arrayIndex, outputList.Count);
        }

        public IEnumerator<NeuralMemory> GetEnumerator()
        {
            return reverseMemoryProbabilityBuffer.Values.ToList().GetEnumerator();
        }

        public bool Remove(NeuralMemory item)
        {
            try
            {
                double itemPriority = memoryProbabilityBuffer[item];
                memoryProbabilityBuffer.Remove(item);
                reverseMemoryProbabilityBuffer.Remove(itemPriority);

                PrioritySum -= itemPriority;
                Count--;

                return true;
            }
            catch
            {
                return false;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return reverseMemoryProbabilityBuffer.Values.ToList().GetEnumerator();
        }
    }

    class DescendingComparer<T> : IComparer<T> where T : IComparable<T>
    {
        public int Compare(T x, T y)
        {
            return y.CompareTo(x);
        }
    }
}
