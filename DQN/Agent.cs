using AlgoTrading.Neural;
using AlgoTrading.SimulatedBroker;
using AlgoTrading.Stocks;
using AlgoTrading.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgoTrading.DQN
{
    public class Agent
    {
        [JsonIgnore]
        public NeuralNetwork TargetNetwork { get; private set; }
        [JsonIgnore]
        public NeuralNetwork OnlineNetwork { get; private set; }
        public NeuralMemoryBuffer MemoryBuffer { get; private set; }
        [JsonIgnore]
        public BrokerEmulator Emulator { get; private set; }
        public AgentSettings Settings { get; private set; }

        double totalIterationReward;
        double totalIterationEstimatedQ;
        double totalLoss;

        int qEstimationsCount;

        int maxExamples = 0;
        int seenEpochExamples;

        int collectedMemoriesCount = 0;

        bool isEpochOver = false;

        public Agent(AgentSettings settings)
        {
            Settings = settings;
            MemoryBuffer = new NeuralMemoryBuffer(Settings.MemoryBuferSize);
        }

        public void SetEmulator(BrokerEmulator emulator)
        {
            Emulator = emulator;

            foreach (List<StockData> stockData in BrokerEmulator.LoadedStocks.Values)
            {
                foreach (StockData stock in stockData)
                {
                    maxExamples += stock.TotalBars;
                }
            }
        }

        public void SetTargetNetwork(NeuralNetwork network)
        {
            TargetNetwork = network;
        }

        public void SetOnlineNetwork(NeuralNetwork network)
        {
            OnlineNetwork = network;
        }

        public void UpdateTargetActorNetwork()
        {
            Dictionary<int, double> onlineActorWeights = OnlineNetwork.CopyWeights();
            Dictionary<int, double> targetActorWeights = TargetNetwork.CopyWeights();

            for(int i = 0; i < targetActorWeights.Count; i++)
            {
                targetActorWeights[i] = onlineActorWeights[i] * Settings.Tau + targetActorWeights[i] * (1 - Settings.Tau);
            }

            TargetNetwork.PasteWeights(targetActorWeights);
        }

        public IterationStatistics RunIteration()
        {
            if (isEpochOver)
            {
                isEpochOver = false;
                seenEpochExamples = 0;
            }

            totalLoss = 0;
            totalIterationReward = 0;
            totalIterationEstimatedQ = 0;

            collectedMemoriesCount = 0;
            qEstimationsCount = 0;

            GainExperience();

            BrokerSessionStatistics sessionStatistics = Emulator.GetSessionStatistics();
            Emulator.ResetSessionStatistics();
            
            if(MemoryBuffer.Count >= maxExamples)
            {
                for (int i = 0; i < 4; i++)
                {
                    List<NeuralMemory> batch = MemoryBuffer.SampleBatch(Settings.BatchSize);
                    Train(GetTrainingSamples(batch));
                }
            }
            
            if (seenEpochExamples >= maxExamples)
            {
                isEpochOver = true;
                Settings.CurrentEpsilon = Math.Max(Settings.MinimumEpsilon, Settings.CurrentEpsilon * Settings.EpsilonDecay);
            }

            return new IterationStatistics(totalIterationEstimatedQ, totalIterationReward, totalLoss, qEstimationsCount, collectedMemoriesCount, isEpochOver,
                sessionStatistics.TotalTrades, sessionStatistics.TotalCommissionedTradeProfit, sessionStatistics.TotalTradeDuration);
        }

        void GainExperience()
        {
            double timeAdvancement = 0;

            List<NeuralMemory> collectedMemories = new List<NeuralMemory>();

            int steps = 0;
            while (steps < Settings.BatchSize + Settings.MemorySteps)
            {               
                Dictionary<string, double> currentState = Emulator.GetCurrentMarketState();
                BrokerTimestepStatistics timestepStatistics = Emulator.GetTimestepStatistics();
                List<string> possibleActions = Emulator.GetCurrentPossibleActions();

                double reward = 0;
                if (timestepStatistics.IsLastActionValid)
                    reward = CalculateReward(timestepStatistics.PositionStartValue, timestepStatistics.EquityChange);

                totalIterationReward += reward;
                seenEpochExamples++;

                ManageCollectedMemories(collectedMemories, reward, currentState, possibleActions, Emulator.IsTerminalCondition);

                if (!Emulator.IsDataOver && !Emulator.IsTerminalCondition)
                {                   
                    string action = SelectActionFromState(currentState, possibleActions);

                    if((Emulator.CurrentBarPos < Emulator.TrackedStock.TotalBars - Settings.MemorySteps) && collectedMemories.Count < Settings.BatchSize)
                    {
                        NeuralMemory newMemory = new NeuralMemory(currentState, possibleActions);
                        newMemory.ExecuteAction(action);
                        collectedMemories.Add(newMemory);
                    }
                        

                    switch (action)
                    {
                        case "Buy":
                            if (Emulator.Purchase(3))
                                timeAdvancement = 1;
                            break;
                        case "Sell":
                            if (Emulator.Sell())
                                timeAdvancement = 1;
                            break;
                        case "Hold":
                            if (Emulator.Hold())
                                timeAdvancement = 1;
                            break;
                            /*case "TrackNext":
                                Emulator.TrackNextStock();
                                timeAdvancement += 0.1;
                                break;
                            case "TrackLast":
                                Emulator.TrackPreviousStock();
                                timeAdvancement += 0.1;
                                break;*/
                    }

                    if (timeAdvancement >= 1)
                    {
                        Emulator.AdvanceTime();
                        timeAdvancement = 0;
                    }
                }
                else
                {
                    timeAdvancement = 0;

                    Emulator.TrackRandomStock();
                    Emulator.Reset();
                }

                steps++;
            }

            MemoryBuffer.Add(collectedMemories);
            collectedMemories = new List<NeuralMemory>();
        }

        string SelectActionFromState(Dictionary<string, double> currentState, List<string> possibleActions)
        {
            float randomGreedy = RandomGenerator.Generate(0, 100000) / 100000;

            string action = String.Empty;

            if (Settings.CurrentEpsilon > randomGreedy)
            {
                randomGreedy = RandomGenerator.Generate(0, possibleActions.Count);

                action = possibleActions.ElementAt((int)randomGreedy);
            }
            else
            {
                OnlineNetwork.FillInputs(currentState);
                OnlineNetwork.ForwardFeed();
                Dictionary<string, double> outputs = OnlineNetwork.GetOutputs();

                KeyValuePair<string, double> actionValue = GetMaxQAction(outputs, possibleActions);

                action = actionValue.Key;
            }

            return action;
        }

        KeyValuePair<string, double> GetMaxQAction(Dictionary<string, double> actionValues, List<string> possibleActions)
        {
            double maxQ = double.MinValue;
            string action = String.Empty;

            foreach (string possibleAction in possibleActions)
            {
                if (actionValues[possibleAction] > maxQ)
                {
                    maxQ = actionValues[possibleAction];
                    action = possibleAction;
                }
            }

            return new KeyValuePair<string, double>(action, maxQ);
        }

        int GetActionIndex(string action)
        {
            if (action == "Buy")
                return 0;
            if (action == "Sell")
                return 1;

            return 2;
        }

        List<NeuralMemory> GetTrainingBatch()
        {
            if (MemoryBuffer.Count >= Settings.BatchSize)
            {
                List<NeuralMemory> batch = new List<NeuralMemory>();
                double randomProbability;

                while (true)
                {
                    foreach (NeuralMemory memory in MemoryBuffer)
                    {
                        randomProbability = RandomGenerator.Generate(0, 1000000) / 1000000;

                        if (randomProbability < MemoryBuffer.GetProbability(memory, Settings.BatchSize))
                            batch.Add(memory);

                        if (batch.Count >= Settings.BatchSize)
                            return batch;
                    }
                }
            }
            else
                return new List<NeuralMemory>();
        }

        List<TrainingSample> GetTrainingSamples(List<NeuralMemory> batch)
        {
            List<TrainingSample> output = new List<TrainingSample>();

            foreach (NeuralMemory memory in batch)
            {
                Dictionary<int, double> targets = new Dictionary<int, double>();
                Dictionary<int, double> finalProbabilities = new Dictionary<int, double>();

                if (OnlineNetwork.Settings.PredictionType == NeuralSettings.PredictionMechanism.NonCategorical)
                {
                    targets.Add(0, 0);
                }
                else
                {
                    foreach (KeyValuePair<int, CategoricalOutputNeuron> neuronPair in OnlineNetwork.CategoricalOutputs.Values.First())
                    {
                        targets.Add(neuronPair.Key, 0);
                        finalProbabilities.Add(neuronPair.Key, 0);
                    }
                }

                double cumulativeRewards = 0;
                double estimatedTarget = 0;

                Dictionary<string, double> learningNetworkOutputs = new Dictionary<string, double>();

                for (int i = memory.States.Count - 2; i >= 0; i--)
                {                   
                    if (memory.EpisodeEnded)
                    {
                        cumulativeRewards += memory.Rewards[i];
                    }
                    else if(i == memory.States.Count - 2)
                    {
                        TargetNetwork.FillInputs(memory.States[i + 1]);
                        TargetNetwork.ForwardFeed();
                        string action = GetMaxQAction(TargetNetwork.GetOutputs(), memory.PossibleActions[i + 1]).Key;

                        OnlineNetwork.FillInputs(memory.States[i + 1]);
                        OnlineNetwork.ForwardFeed();

                        //OnlineCriticNetwork.FillInputs(criticInputs);
                        //OnlineCriticNetwork.ForwardFeed();

                        if(OnlineNetwork.Settings.PredictionType == NeuralSettings.PredictionMechanism.NonCategorical)
                        {
                            targets[0] = Math.Pow(Settings.Discount, i) * OnlineNetwork.GetOutputs()[action];
                        }
                        else
                        {
                            foreach(KeyValuePair<int, CategoricalOutputNeuron> neuronPair in OnlineNetwork.CategoricalOutputs[action])
                            {
                                targets[neuronPair.Key] = Settings.Discount * neuronPair.Value.RelatedSoftMaxOutput.Value;
                                finalProbabilities[neuronPair.Key] = targets[neuronPair.Key];
                            }

                            estimatedTarget = Math.Pow(Settings.Discount, i) * OnlineNetwork.GetOutputs()[action];
                        }                    
                    }
                    else
                    {
                        if(i != 0)
                        {
                            cumulativeRewards += memory.Rewards[i] * Math.Pow(Settings.Discount, i);
                        }
                        else
                        {
                            cumulativeRewards += memory.Rewards[i];
                        }
                    }
                }

                /*OnlineCriticNetwork.FillInputs(criticInputs);
                OnlineCriticNetwork.ForwardFeed();*/

                OnlineNetwork.FillInputs(memory.States[0]);
                OnlineNetwork.ForwardFeed();

                double actionQ = OnlineNetwork.GetOutputs()[memory.Actions[0]];
                double temporalDifference = (estimatedTarget + cumulativeRewards) - actionQ;

                memory.SetTemporalDifference(temporalDifference);
                MemoryBuffer.UpdatePriority(memory);

                //totalLoss += memory.AbsoluteTemporalDifference;

                totalLoss += NeuralMath.HuberLoss(temporalDifference);

                for(int i = 0; i < targets.Count; i++)
                {
                    targets[i] += cumulativeRewards;

                    if(OnlineNetwork.Settings.PredictionType == NeuralSettings.PredictionMechanism.CategoricalCrossEntropy)
                        targets[i] = Math.Min(Math.Max(OnlineNetwork.Settings.MinV, targets[i]), OnlineNetwork.Settings.MaxV);
                }

                if(OnlineNetwork.Settings.PredictionType == NeuralSettings.PredictionMechanism.NonCategorical)
                    output.Add(new TrainingSample(targets, memory.Actions[0], memory.States[0]));
                else
                    output.Add(new TrainingSample(targets, finalProbabilities, memory.Actions[0], memory.States[0]));
            }

            return output;
        }

        void Train(List<TrainingSample> trainingSamples)
        {
            foreach (TrainingSample sample in trainingSamples)
            {
                qEstimationsCount++;

                //QuantileBackpropagationSpecification specification = new QuantileBackpropagationSpecification(sample.Action, sample.TemporalDifference);
                //CrossEntropyBackpropagationSpecification specification = new CrossEntropyBackpropagationSpecification(sample.Action, sample.Targets, sample.TargetProbabilities);
                HuberLossBackpropagationSpecification specification = new HuberLossBackpropagationSpecification(sample.Action, sample.Targets.Values.First());

                OnlineNetwork.FillInputs(sample.State, false);
                OnlineNetwork.ForwardFeed();
                OnlineNetwork.Backpropagate(specification);
                OnlineNetwork.UpdateWeights();              
            }
        }

        double CalculateReward(double positionValue, double equityChange)
        {
            //double valueCoef = Math.Tanh((rewardVariables["Cash"] + rewardVariables["Equity"] - rewardVariables["StartingCash"])
            //   / (rewardVariables["StartingCash"]));
            //double equityChangeCoef = Math.Tanh(rewardVariables["EquityChange"] / (0.3d * rewardVariables["StartingCash"])) + 1;
            //double validityCoef = Math.Min(Math.Tanh(rewardVariables["Validity"] / 2) + 0.55d, 1);

            double equityChangeCoef = 0;

            if (positionValue > 0)
                equityChangeCoef = Math.Tanh(equityChange / (0.3d * positionValue));

            double reward = equityChangeCoef; //* validityCoef;

            return reward;
        }

        void ManageCollectedMemories(List<NeuralMemory> memories, double currentReward, Dictionary<string, double> currentState, List<string> currentPossibleActions,
            bool isEpisodeTerminated = false)
        {
            for (int i = 0; i < memories.Count; i++)
            {
                if (!memories[i].Complete)
                {
                    memories[i].PerformTransition(currentReward, currentState, currentPossibleActions);

                    if (isEpisodeTerminated)
                    {
                        memories[i].CompleteEpisode();
                        memories[i].CompleteMemory();
                    }
                    else if (memories[i].TimeStep == Settings.MemorySteps - 1)
                    {
                        memories[i].CompleteMemory();
                    }
                }
            }
        }
    }


    public class AgentSettings
    {
        [JsonProperty]
        public string AgentName { get; private set; }
        [JsonProperty]
        public int MemoryBuferSize { get; private set; }
        [JsonProperty]
        public int BatchSize { get; private set; }
        [JsonProperty]
        public double Discount { get; private set; }
        [JsonProperty]
        public double InitialEpsilon { get; private set; }
        [JsonProperty]
        public double CurrentEpsilon { get; set; }
        [JsonProperty]
        public double MinimumEpsilon { get; private set; }
        [JsonProperty]
        public double EpsilonDecay { get; private set; }
        [JsonProperty]
        public int MemorySteps { get; private set; }
        [JsonProperty]
        public double Tau { get; private set; }

        public AgentSettings(string agentName, int bufferSize, int batchSize, double discount, double epsilon, double minEpsilon, double epsilonDecay, int memorySteps, double tau)
        {
            AgentName = agentName;
            MemoryBuferSize = bufferSize;
            Discount = discount;
            BatchSize = batchSize;
            InitialEpsilon = epsilon;
            CurrentEpsilon = epsilon;
            MinimumEpsilon = minEpsilon;
            EpsilonDecay = epsilonDecay;
            MemorySteps = memorySteps;
            Tau = tau;
        }
    }
}
