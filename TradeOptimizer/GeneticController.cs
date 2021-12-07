using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;
using System.Collections;

using AlgoTrading.TradeOptimizer.Genetics;
using AlgoTrading.TradeOptimizer.Interfaces;
using AlgoTrading.TradeOptimizer.Backtesters;
using AlgoTrading.DataKeeping;

namespace AlgoTrading.TradeOptimizer
{
    internal class GeneticController
    {
        public List<IStrategyBacktester> StrategyBacktesters { get; private set; }
        public GeneticAlgorithm GeneticAlgorithm { get; private set; }
        
        public GenerationSaveManager GenSaveManager { get; private set; }
        public MultithreadController ThreadController { get; private set; }
        public enum BacktestMode { PineScriptBacktest, PythonBacktest}

        public BacktestMode ControllerMode { get; private set; }
        public string IndexFileName { get; private set; } = String.Empty;
        public int BacktesterCount { get; private set; } = 6;
        public string ScriptName { get; private set; }

        public GeneticController(string scriptName, BacktestMode mode, string[] args) : this(scriptName, mode)
        {                    
            if(args.Length > 0)
            {
                foreach(string argument in args)
                {
                    string[] commandSplits = argument.Split('=');
                    switch (commandSplits[0])
                    {
                        case "setIndexFile":
                            IndexFileName = commandSplits[1];
                            break;
                        case "btCount":
                            BacktesterCount = Convert.ToInt32(commandSplits[1]);
                            break;
                    }
                        
                }
            }
        }

        public GeneticController(string scriptName, BacktestMode mode)
        {
            ControllerMode = mode;
            ScriptName = scriptName;

            StrategyBacktesters = new List<IStrategyBacktester>();

            if (mode == BacktestMode.PineScriptBacktest)
                for (int i = 0; i < BacktesterCount; i++)
                {
                    //StrategyBacktesters.Add(new PineScriptBacktester(ScriptName));
                }
            else
                for (int i = 0; i < BacktesterCount; i++)
                {
                    StrategyBacktesters.Add(new PythonScriptBacktester(ScriptName));
                }

            ThreadController = new MultithreadController(StrategyBacktesters);
        }

        public void CreateInitialGenetics(int popCap, float similarityIndex, float crossoverChance, float deviation, float selection)
        {
            GeneticSettings settings = new GeneticSettings(popCap, StrategyBacktesters.First().GetStrategyIndexes(), similarityIndex, crossoverChance, deviation, selection);
            GeneticAlgorithm = new GeneticAlgorithm(settings);

            GenSaveManager = new GenerationSaveManager(ScriptName, GeneticAlgorithm);
            GenSaveManager.SaveLastGeneration();
        }
        
        public bool RunGeneration()
        {
            int individualCount = 0;
            GeneticAlgorithm.MinProfit = float.MaxValue;
            GeneticAlgorithm.MaxProfit = float.MinValue;            

            foreach(Individual individual in GeneticAlgorithm.Individuals)
            {               
                if (!individual.AlreadyTested)
                {
                    ThreadController.AddIndividual(individual);
                }
                else
                {
                    individualCount++;
                    GeneticAlgorithm.SetMaximums(individual);
                    GeneticAlgorithm.DiscretizizeIndividual(individual);
                }
            }

            Task backtestProcessing = Task.Run(() => ThreadController.StartProcessBacktests());

            while(!ThreadController.IsProcessing && !(ThreadController.IndividualResultPool.Count == 0 && ThreadController.InputIndividualPool.Count == 0))
            {
                Thread.Sleep(50);
            }

            while (ThreadController.IsProcessing)
            {
                if (ThreadController.IndividualResultPool.Count > 0)
                {
                    individualCount++;

                    Individual individual = ThreadController.IndividualResultPool.Take();

                    if (individual.HasFatalError)
                    {
                        individual.Profit = GeneticAlgorithm.MinProfit;
                        individual.Sharpe = GeneticAlgorithm.MinSharpe;
                        individual.AvgHours = GeneticAlgorithm.MaxAvgHours;
                        individual.TradeCount = GeneticAlgorithm.MinTrades;
                    }

                    GeneticAlgorithm.DiscretizizeIndividual(individual);

                    ConsoleColor consoleColor = ConsoleColor.Gray;

                    int relative = GeneticAlgorithm.SetMaximums(individual);

                    if (relative == 1)
                    {
                        consoleColor = ConsoleColor.DarkGreen;
                        Console.Beep(1318, 100);
                    }
                        
                    if (relative == -1)
                        consoleColor = ConsoleColor.Red;

                    if (!individual.HasFatalError)
                    {
                        Console.ForegroundColor = consoleColor;
                        Console.WriteLine("[" + individualCount + "] Individual ID " + individual.ID + " tested: \n" + "Fitness: " + individual.FitnessScore + 
                    "; Profit: " + individual.Profit + "\nProfit Factor: " + individual.ProfitFactor + "; Sharpe: " + individual.Sharpe +
                    "\nAvg Hours: " + individual.AvgHours + "; Trades: " + individual.TradeCount);
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("[" + individualCount + "] Individual ID " + individual.ID + " evaluated as fatal and will be exterminated.");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                else
                    Thread.Sleep(10);
            }

            float previousAvgFitness = GeneticAlgorithm.AverageFitness;

            int newOffspring = GeneticAlgorithm.AdvanceGeneration();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Generation " + GeneticAlgorithm.GenerationCount + ": \n" +
                "Fittest Profit: " + GeneticAlgorithm.BestIndividual.Profit + "\nLeast Fit Profit: " + GeneticAlgorithm.WorstIndividual.Profit +
                "\nMax fitness: " + GeneticAlgorithm.MaxFitness + "; Avg. Fitness: " + GeneticAlgorithm.AverageFitness +
                "\nAverage mutation chance: " + GeneticAlgorithm.AverageMutationChance + "; Average similarity: " + GeneticAlgorithm.AverageSimilarity);
            Console.ForegroundColor = ConsoleColor.Gray;

            if(GeneticAlgorithm.AverageFitness - previousAvgFitness > 0)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Average fitness increased!");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            if (GeneticAlgorithm.AverageFitness - previousAvgFitness < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Average fitness decreased!");
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            GeneticAlgorithm.GenerationCount++;
            GenSaveManager.SaveLastGeneration();

            if (newOffspring == 0)
                return false;
            else
                return true;
        }

        public void Run()
        {
            while (RunGeneration()) ;
                
        }

    }
}
