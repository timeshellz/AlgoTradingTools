using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrading.Utilities;

namespace AlgoTrading.TradeOptimizer.Genetics
{
    public class Individual
    {
        public static int LastUsedID { get; private set; } = 0;
        public int ID { get; private set; }
        public Chromosome Chromosome { get; private set; }
        public float FitnessScore { get; set; }
        public float RankSelectionProbability { get; set; }
        public float Profit { get; set; }
        public float ProfitFactor { get; set; }
        public float Sharpe { get; set; }
        public float AvgHours { get; set; }
        public float TradeCount { get; set; }
        public bool GetsToMate { get; set; } = false;
        public bool AlreadyTested { get; set; } = false;
        public bool HasFatalError { get; set; } = false;
        public float MutationChance { get; set; }

        public Individual(Chromosome chromosome)
        {
            ID = LastUsedID++;
            Chromosome = chromosome;
        }

        public Individual CrossOver(Individual otherIndividual)
        {
            float randomRoll = 0;
            float birthFatalityChance = 0;

            Gene[] offspringGenes = (Gene[])Chromosome.Genes.Clone();
            Gene[] otherParentGenes = otherIndividual.Chromosome.Genes;

            int crossoverPointCount = (int)RandomGenerator.Generate(1, 3);
            int sameGeneCount = 0;

            for (int g = 0; g < Chromosome.Genes.Length; g++)
            {
                if (offspringGenes[g].Value == otherParentGenes[g].Value)
                    sameGeneCount++;
            }

            //birthFatalityChance += (float)Math.Exp(sameGeneCount - 26) / 500;
            int lastCrossoverPointEnd = 0;

            for (int z = 0; z < crossoverPointCount; z++)
            {
                int crossoverStartPoint = (int)RandomGenerator.Generate(lastCrossoverPointEnd, Chromosome.Genes.Length - 1);
                int crossoverEndPoint = (int)RandomGenerator.Generate(crossoverStartPoint + 1, Chromosome.Genes.Length);
                lastCrossoverPointEnd = crossoverEndPoint;

                for (int j = crossoverStartPoint; j < crossoverEndPoint; j++)
                {
                    offspringGenes[j].Value = otherParentGenes[j].Value;
                }
            }

            randomRoll = ((float)RandomGenerator.Generate(0, 100000)) / 100000;

            if (randomRoll < ((MutationChance + otherIndividual.MutationChance) / 2))
            {
                int mutationStartPoint = (int)RandomGenerator.Generate(0, Chromosome.Genes.Length);
                int mutationEndPoint = (int)RandomGenerator.Generate(mutationStartPoint + 1, Chromosome.Genes.Length);

                Gene[] mutationRange = (Gene[])offspringGenes.Clone();

                int z = mutationEndPoint - 1;
                for (int j = mutationStartPoint; j < mutationEndPoint; j++)
                {
                    offspringGenes[j].Value = mutationRange[z].Value;
                    z--;
                }

                if(randomRoll < MutationChance/10)
                {
                    int randomPosition = (int)RandomGenerator.Generate(0, Chromosome.Genes.Length);
                
                    offspringGenes[randomPosition].Value
                        = RandomGenerator.Generate((int)offspringGenes[randomPosition].Value - 1,
                        (int)offspringGenes[randomPosition].Value + 1);
                }
            }

            Chromosome offspringChromosome = new Chromosome(offspringGenes);
            Individual newIndividual = new Individual(offspringChromosome);

            if (randomRoll < birthFatalityChance)
                //newIndividual.HasFatalError = true;

            GetsToMate = false;

            return newIndividual;
        }

        public Dictionary<string, float> GetIndexes()
        {
            Dictionary<string, float> indexes = new Dictionary<string, float>();

            foreach (Gene gene in Chromosome.Genes)
            {
                indexes.Add(gene.Name, gene.Value);               
            }

            return indexes;
        }

        public void SetStatValues(Dictionary<string, float> values)
        {
            AlreadyTested = true;

            if (!HasFatalError)
            {
                Profit = values["Profit"];
                ProfitFactor = values["Profit Factor"];
                Sharpe = values["Sharpe"];
                AvgHours = values["Duration"];
                TradeCount = values["Trade Count"];               
                CalculateFitness();
            }
            else
            {
                FitnessScore = 0;
            }
            
        }

        public void AdaptMutation(float similarityIndex, int populationCap, int representation)
        {
            float mutationDelta = 1f / (15 * similarityIndex * populationCap * representation) * (float)Math.Pow(representation - similarityIndex * populationCap, 3) * 1f / Chromosome.Genes.Length;

            MutationChance = Math.Min(Math.Max(1f / Chromosome.Genes.Length, mutationDelta), 2*similarityIndex);
        }
        
        public void CalculateFitness()
        {

            float power = (float)Math.Pow((float)Math.Abs(Profit), 1f / 3f) / 4f;
            if (Profit < 0)
                power *= -1;
            float profitCoef = (float)Math.Exp(power);
            float avgHoursCoef = 1.95f*(float)Math.Exp(-(float)Math.Pow(AvgHours-12, 2)/(float)Math.Pow(1000, 2)) + 0.05f;
            float sharpeCoef = 2 * (float)Math.Sin(Math.Max(0, Math.Min(Sharpe, 6)) - 1.4f) + 2.1f;
            float tradeCountCoef = 1;
            float profitFactorCoef = (float)Math.Sin(Math.Max(0, Math.Min(ProfitFactor, 6)) - 1.5f) + 1.1f;

            if (Profit >= 0)
            {
                tradeCountCoef = (float)Math.Pow(TradeCount, 2) / 4000 + 1;
                //avgHoursCoef = 1f / (float)Math.Pow(AvgHours + 1, 1f / 14f);               
            }

            FitnessScore = (float)Math.Pow(profitCoef * avgHoursCoef * sharpeCoef * tradeCountCoef * profitFactorCoef, 1.4);


        }
    }
}
