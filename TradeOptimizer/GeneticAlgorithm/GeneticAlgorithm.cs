using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AlgoTrading.Utilities;

namespace AlgoTrading.TradeOptimizer.Genetics
{
    public class GeneticAlgorithm
    {
        public int GenerationCount { get; set; } = 0;
        public Dictionary<string, float> BestIndexes { get; set; }
        public float MaxProfit { get; set; } = float.MinValue;
        public float MinProfit { get; set; } = float.MaxValue;        
        public float MaxSharpe { get; set; } = float.MinValue;
        public float MinSharpe { get; set; } = float.MaxValue;
        public float MaxAvgHours { get; set; } = float.MinValue;
        public float MinAvgHours { get; set; } = float.MaxValue;
        public float MaxFitness { get; set; } = float.MinValue;
        public float MinFitness { get; set; } = float.MaxValue;
        public float MaxSQN { get; set; } = float.MinValue;
        public float MinSQN { get; set; } = float.MaxValue;
        public float MaxTrades { get; set; } = float.MinValue;
        public float MinTrades { get; set; } = float.MaxValue;
        public float AverageFitness { get; set; }
        public float AverageMutationChance { get; set; }
        public float AverageSimilarity { get; set; }
        public GeneticSettings Settings { get; private set; }
        public List<Individual> Individuals { get; set; }
        public Individual BestIndividual { get; set; }

        public Individual WorstIndividual { get; set; }
        public Dictionary<float, int> DiscreteFitnessBands { get; set; }
        

        public GeneticAlgorithm(GeneticSettings settings)
        {
            Settings = settings;

            CreateInitialIndividuals();
        }

        void CreateInitialIndividuals()
        {
            Individuals = new List<Individual>();
            DiscreteFitnessBands = new Dictionary<float, int>();

            while(Individuals.Count < Settings.PopulationCap)
            {
                List<Gene> genes = new List<Gene>();

                foreach(KeyValuePair<string, float> index in Settings.InitialIndexes)
                {
                    float randomValue = (float)(RandomGenerator.Generate((int)(-100000*Settings.InitialDeviation), (int)(100000 * Settings.InitialDeviation)))/100000;

                    genes.Add(new Gene(index.Key, index.Value - randomValue));
                }

                Individuals.Add(new Individual(new Chromosome(genes.ToArray())));
            }
        }


        public void DiscretizizeIndividual(Individual individual)
        {
            if (DiscreteFitnessBands.Count == 0)
                DiscreteFitnessBands.Add(individual.FitnessScore, 1);
            else
            {
                bool isValueFound = false;

                for (int i = 0; i < DiscreteFitnessBands.Count; i++)
                {
                    float key = DiscreteFitnessBands.ElementAt(i).Key;

                    if (IsFitnessWithinBand(individual.FitnessScore, key))
                    {
                        DiscreteFitnessBands[key] += 1;
                        isValueFound = true;

                        break;
                    }
                }

                if (!isValueFound)
                    DiscreteFitnessBands.Add(individual.FitnessScore, 1);
            }
        }

        void AdaptAllMutations()
        {
            float mutationSum = 0;
            float averageRepresentation = 0;

            foreach (Individual individual in Individuals)               
            {
                float bandValue = 0;

                foreach (KeyValuePair<float, int> fitnessBand in DiscreteFitnessBands)
                {
                    if (IsFitnessWithinBand(individual.FitnessScore, fitnessBand.Key))
                    {
                        bandValue = fitnessBand.Value;

                        individual.AdaptMutation(Settings.SimilarityIndex, Settings.PopulationCap, fitnessBand.Value);
                        break;
                    }                                            
                }

                mutationSum += individual.MutationChance;
            }

            foreach(int representation in DiscreteFitnessBands.Values)
            {
                averageRepresentation += representation;
            }

            averageRepresentation /= DiscreteFitnessBands.Count;

            AverageSimilarity = averageRepresentation / Settings.PopulationCap;
            AverageMutationChance = mutationSum / Individuals.Count;
            DiscreteFitnessBands.Clear();
        }

        public int AdvanceGeneration()
        {
            SortIndividualsAscending();
            RankIndividuals();
            AdaptAllMutations();
            List<Individual> offspring = SelectivelyCreateOffspring((int)(Settings.SelectionFraction * Settings.PopulationCap));

            int overCap = (Individuals.Count + offspring.Count) - Settings.PopulationCap;
            if (overCap > 0)
                DecreaseUnfitPopulation(overCap);

            Individuals.AddRange(offspring);
            //FixFatalErrors();
                      
            return offspring.Count;
        }

        void RankIndividuals()
        {
            float fitnessSum = 0;
            float sumOfRanks = (Individuals.Count + 1) * (Individuals.Count / 2);

            for(int i = 0; i < Individuals.Count; i++)
            {
                fitnessSum += Individuals[i].FitnessScore;

                Individuals[i].RankSelectionProbability = i / sumOfRanks;
            }

            AverageFitness = fitnessSum / Individuals.Count;
        }

        void SelectIndividuals(int count)
        {
            int selectedCount = 0;

            while(selectedCount < count)
            {
                foreach (Individual individual in Individuals)
                {
                    float randomValue = ((float)(RandomGenerator.Generate(0, 100000)) / 100000);

                    if (individual.RankSelectionProbability >= randomValue && !individual.HasFatalError)
                    {
                        individual.GetsToMate = true;
                        selectedCount++;

                        if (selectedCount == count)
                        {
                            return;
                        }
                    }

                }

                if (selectedCount == -1)
                    return;
            }            
        }

        List<Individual> CreateOffspring(int count)
        {
            List<Individual> matingIndividuals = Individuals.Where(e => e.GetsToMate).ToList();
            List<Individual> newOffspring = new List<Individual>();

            if (matingIndividuals.Count == 0)
                return new List<Individual>();

            int limiter = count;
            if (count == -1)
                limiter = matingIndividuals.Count;        

            for (int i = 0; i < limiter; i++)
            {
                int randomValue;

                while (true)
                {
                    if (matingIndividuals.Count == 1)
                        return newOffspring;

                    randomValue = (int)RandomGenerator.Generate(0, matingIndividuals.Count);
                    if (randomValue != i)
                        break;
                }

                float randomRoll = (float)RandomGenerator.Generate(0, 100000) / 100000;

                if (randomRoll <= Settings.CrossoverChance || count != -1)
                    newOffspring.Add(matingIndividuals.ElementAt(Math.Min(i, matingIndividuals.Count - 1)).CrossOver(matingIndividuals.ElementAt(randomValue)));
            }

            return newOffspring;
        }

        List<Individual> SelectivelyCreateOffspring()
        {
            List<Individual> offspring = new List<Individual>();

            while (offspring.Count == 0)
            {
                SelectIndividuals(-1);
                offspring = CreateOffspring(-1);
            }

            return offspring;
        }

        List<Individual> SelectivelyCreateOffspring(int selectionCount)
        {
            if (selectionCount == -1)
                return SelectivelyCreateOffspring();

            List<Individual> offspring = new List<Individual>();

            SelectIndividuals(selectionCount);
            offspring = CreateOffspring(-1);

            return offspring;
        }

        void DecreaseUnfitPopulation(int amountOverCap)
        {
            float fitnessCutoff = (float)amountOverCap / (float)Settings.PopulationCap;

            Individuals.RemoveRange(0, amountOverCap);           
        }

        void SortIndividualsAscending()
        {
            Individuals.Sort((e, o) =>
            {
                if (o.FitnessScore > e.FitnessScore)
                    return -1;
                else if (o.FitnessScore == e.FitnessScore)
                    return 0;
                else return 1;
            });
        }

        void FixFatalErrors()
        {
            int removed = Individuals.RemoveAll(e => e.HasFatalError);

            if (removed > 0)
                Individuals.AddRange(SelectivelyCreateOffspring(removed));
        }

        public int SetMaximums(Individual individual)
        {
            int fitnessRelative = 0;

            if (individual.Profit < MinProfit)
            {
                MinProfit = individual.Profit;
            }
            if (individual.Profit > MaxProfit)
            {
                MaxProfit = individual.Profit;
            }
            if (individual.Sharpe > MaxSharpe)
            {
                MaxSharpe = individual.Sharpe;
            }
            if (individual.Sharpe < MinSharpe)
            {
                MinSharpe = individual.Sharpe;
            }
            if (individual.AvgHours > MaxAvgHours)
            {
                MaxAvgHours = individual.AvgHours;
            }
            if (individual.AvgHours < MinAvgHours)
            {
                MinAvgHours = individual.AvgHours;
            }
            if (individual.TradeCount < MinTrades)
            {
                MinTrades = individual.TradeCount;
            }
            if (individual.TradeCount > MaxTrades)
            {
                MaxTrades = individual.TradeCount;
            }
            if (individual.FitnessScore > MaxFitness)
            {
                MaxFitness = individual.FitnessScore;
                BestIndividual = individual;
                BestIndexes = individual.GetIndexes();
                fitnessRelative = 1;
            }
            if (individual.FitnessScore < MinFitness)
            {
                MinFitness = individual.FitnessScore;
                WorstIndividual = individual;
                fitnessRelative = -1;
            }

            return fitnessRelative;
        }

        bool IsFitnessWithinBand(float fitnessScore, float fitnessBand)
        {
            if (fitnessScore >= (fitnessBand - fitnessBand * (Settings.SimilarityIndex / 8)) && fitnessScore <= (fitnessBand + fitnessBand * (Settings.SimilarityIndex / 8)))
                return true;
            else
                return false;
        }
    }

    public class GeneticSettings
    {
        public int PopulationCap { get; private set; } 
        public Dictionary<string, float> InitialIndexes { get; private set; }
        public float InitialDeviation { get; private set; }
        public float CrossoverChance { get; private set; }
        public float SimilarityIndex { get; private set; }
        public float SelectionFraction { get; private set; }

        public GeneticSettings(int populationCap, Dictionary<string, float> initialIndexes, float similarityIndex,
            float crossoverChance, float initialDeviation, float selectionFraction)
        {
            PopulationCap = populationCap;
            InitialIndexes = initialIndexes;
            InitialDeviation = initialDeviation;
            CrossoverChance = crossoverChance;
            SimilarityIndex = similarityIndex;
            SelectionFraction = selectionFraction;

            if (InitialDeviation < 0)
                InitialDeviation *= -1;

            if (SimilarityIndex < 0)
                SimilarityIndex *= -1;

            if (SelectionFraction < 0)
                SelectionFraction *= -1;

            while (SimilarityIndex > 1)
                SimilarityIndex /= 10;

            while (CrossoverChance > 1)
                CrossoverChance /= 10;

            while (SelectionFraction > 1)
                SelectionFraction /= 10;
        }
    }

}
