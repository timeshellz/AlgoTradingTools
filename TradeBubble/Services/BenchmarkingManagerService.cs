using AlgoTrading.Neural.Persistence;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace TradeBubble.Services
{
    public class BenchmarkingManagerService : BackgroundService
    {
        private INeuralPersistenceManager neuralPersistenceManager;

        private static ConcurrentQueue<string> neuralNetworkBenchmarkQueue = new ConcurrentQueue<string>();

        public BenchmarkingManagerService(INeuralPersistenceManager neuralPersistenceManager)
        {
            this.neuralPersistenceManager = neuralPersistenceManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                List<Task> queueManageTasks = new List<Task>();

                queueManageTasks.Add(Task.Run(() => ManageBenchmarkQueue(neuralNetworkBenchmarkQueue, stoppingToken)));

                await Task.WhenAll(queueManageTasks);
            }
        }

        private async Task ManageBenchmarkQueue(ConcurrentQueue<string> queue, CancellationToken stoppingToken)
        {
            while (true)
            {
                while (queue.IsEmpty)
                    await Task.Delay(300);

                string networkName;
                queue.TryDequeue(out networkName);

                await RunBenchmark(networkName);

                if (stoppingToken.IsCancellationRequested)
                    break;
            }
        }

        private async Task CreateBenchmarkDirector()
        {
            //BenchmarkingAgent agent = new BenchmarkingAgent(new BenchmarkingAgentConfiguration(), new NeuralNetwork(), new Broker())
            //BenchmarkingAgentDirector director = new BenchmarkingAgentDirector()
        }

        private async Task RunBenchmark(string networkName)
        {
            var network = neuralPersistenceManager.LoadNeuralNetwork(networkName);


        }
    }
}
