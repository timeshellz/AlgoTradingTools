using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AlgoTrading.TradeOptimizer.Genetics;
using AlgoTrading.TradeOptimizer.Interfaces;

namespace AlgoTrading.TradeOptimizer
{
    internal class MultithreadController
    {
        public BlockingCollection<Individual> InputIndividualPool { get; private set; }
        public BlockingCollection<Individual> IndividualResultPool { get; private set; }
        public List<IStrategyBacktester> Backtesters { get; private set; }
        public bool IsProcessing { get; private set; } = false;

        public MultithreadController(List<IStrategyBacktester> backtesters)
        {
            InputIndividualPool = new BlockingCollection<Individual>();
            IndividualResultPool = new BlockingCollection<Individual>();
            Backtesters = backtesters;
        }

        public void AddIndividual(Individual individual)
        {
            InputIndividualPool.Add(individual);
        }

        public void StartProcessBacktests()
        {
            StartProcessBacktests(-1);
        }

        public void StartProcessBacktests(int count)
        {
            if (count == -1)
                count = InputIndividualPool.Count;


            IsProcessing = true;
            while (InputIndividualPool.Count > 0 && count > 0)
            {
                List<Task<Individual>> tasks = new List<Task<Individual>>();

                foreach (IStrategyBacktester backtester in Backtesters)
                {
                    if (InputIndividualPool.Count > 0)
                        tasks.Add(BacktestIndividualTask(backtester));
                    else
                        break;
                }

                Task<Individual[]> taskResults = Task.Run(async () => await Task.WhenAll(tasks));

                foreach (Individual resultIndividual in taskResults.Result)
                {
                    IndividualResultPool.Add(resultIndividual);
                }                
            }

            while (IndividualResultPool.Count > 0)
                Task.Delay(100);

            IsProcessing = false;
        }

        async Task<Individual> BacktestIndividualTask(IStrategyBacktester backtester)
        {
            Individual individual = InputIndividualPool.Take();
            Dictionary<string, float> indexes = individual.GetIndexes();
            Dictionary<string, float> results = new Dictionary<string, float>();

            try
            {
                Exception exceptionOut = null;

                results = await Task.Run(() =>
                {
                    try
                    {
                       return backtester.RunBacktest(indexes);
                    }
                    catch(Exception inner)
                    {
                        exceptionOut = inner;
                        return new Dictionary<string, float>();
                    }
                });

                if (exceptionOut != null)
                    throw exceptionOut;
            }
            catch
            {
                individual.HasFatalError = true;
            }

            individual.SetStatValues(results);

            return individual;
        }

    }
}
