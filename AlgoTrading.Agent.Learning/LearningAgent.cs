﻿using AlgoTrading.Neural;
using AlgoTrading.Broker;
using AlgoTrading.Utilities;
using AlgoTrading.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoTrading.Agent.Learning
{
    public class LearningAgent : TradingAgent, IStatisticsProvider<LearningAgentStatistics>
    {
        public NeuralNetwork TargetNetwork { get; private set; }
        public NeuralMemoryBuffer MemoryBuffer { get; private set; }
        public LearningAgentConfiguration Configuration { get; private set; }

        private List<NeuralMemory> collectedInteractionMemories { get; set; } = new List<NeuralMemory>();

        private LearningAgentStatistics statistics = new LearningAgentStatistics();

        public LearningAgent(LearningAgentConfiguration configuration, 
            NeuralNetwork targetNetwork, NeuralNetwork onlineNetwork, IBroker broker) : base(onlineNetwork, broker)
        {
            Configuration = configuration;

            MemoryBuffer = new NeuralMemoryBuffer(Configuration.MemoryBuferSize);
            TargetNetwork = targetNetwork;
        }

        public override async Task<bool> Interact()
        {
            MarketState currentState = await Broker.GetNextTimestep();
            List<BrokerAction> possibleActions = Broker.GetAvailableActions();

            double reward = 0;

            //if (currentState.PreviousPosition != null && currentState.PreviousPosition is OpenPosition openPosition)
            //    reward = CalculateReward(openPosition.OpeningValue, 
            //        openPosition.GetCurrentValueChange(currentState.CurrentStockBar),
            //        openPosition.Commission);

            if (currentState.CurrentPosition != null && currentState.CurrentPosition is OpenPosition openPosition)
                reward = CalculateReward(openPosition.OpeningValue, openPosition.GetPositionValue(currentState.CurrentStockBar));

            ProcessCollectedMemories(reward, currentState.ToDictionary(), possibleActions.GetActionStrings(), possibleActions.Count == 0);

            if (possibleActions.Count > 0)
            {
                BrokerAction action = SelectActionFromState(currentState, possibleActions);

                NeuralMemory newMemory = new NeuralMemory(currentState.ToDictionary(), possibleActions.GetActionStrings());
                newMemory.ExecuteAction(action.GetActionString());

                collectedInteractionMemories.Add(newMemory);
                MemoryBuffer.Add(newMemory);
                statistics.MemoriesCollected++;

                await EmulateAction(action);
                
                statistics.RecordInteraction(reward, 1);

                return true;
            }

            statistics.RecordInteraction(reward, 0);

            return false;
        }       

        public void DecayEpsilon()
        {
            Configuration.CurrentEpsilon = Math.Max(Configuration.MinimumEpsilon, 
                Configuration.CurrentEpsilon * Configuration.EpsilonDecay);
        }

        public void SetEpsilon(double newEpsilon)
        {
            Configuration.CurrentEpsilon = Math.Max(0, Math.Min(newEpsilon, 1));
        }

        public double GetEpsilon()
        {
            return Configuration.CurrentEpsilon;
        }

        public void UpdateTargetNetwork()
        {
            Dictionary<int, double> onlineActorWeights = OnlineNetwork.CopyWeights();
            Dictionary<int, double> targetActorWeights = TargetNetwork.CopyWeights();

            for (int i = 0; i < targetActorWeights.Count; i++)
            {
                targetActorWeights[i] = onlineActorWeights[i] * Configuration.Tau + targetActorWeights[i] * (1 - Configuration.Tau);
            }

            TargetNetwork.PasteWeights(targetActorWeights);
        }

        protected override BrokerAction SelectActionFromState(MarketState marketState, List<BrokerAction> possibleActions)
        {
            float randomGreedy = RandomGenerator.Generate(0, 100000) / 100000;

            BrokerAction action;

            if (Configuration.CurrentEpsilon > randomGreedy)
            {
                randomGreedy = RandomGenerator.Generate(0, possibleActions.Count);

                action = possibleActions.ElementAt((int)randomGreedy);
            }
            else
            {
                action = base.SelectActionFromState(marketState, possibleActions);
            }

            return action;
        }

        private List<TrainingSample> GetTrainingSamples(List<NeuralMemory> batch)
        {
            List<TrainingSample> output = new List<TrainingSample>();

            foreach (NeuralMemory memory in batch)
            {
                if (memory.States.Count != Configuration.MemorySteps)
                    continue;

                Dictionary<int, double> targets = new Dictionary<int, double>();

                targets.Add(0, 0);

                double cumulativeRewards = 0;

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

                        //TODO: optimize
                        string action = GetMaxQAction(TargetNetwork.GetOutputs(), memory.PossibleActions[i + 1].GetBrokerActions()).Key.GetActionString();

                        OnlineNetwork.FillInputs(memory.States[i + 1]);
                        OnlineNetwork.ForwardFeed();

                        //OnlineCriticNetwork.FillInputs(criticInputs);
                        //OnlineCriticNetwork.ForwardFeed();

                        targets[0] = Math.Pow(Configuration.Discount, i) * OnlineNetwork.GetOutputs()[action];
                    }
                    else
                    {
                        if(i != 0)
                        {
                            cumulativeRewards += memory.Rewards[i] * Math.Pow(Configuration.Discount, i);
                        }
                        else
                        {
                            cumulativeRewards += memory.Rewards[i];
                        }
                    }
                }

                OnlineNetwork.FillInputs(memory.States[0]);
                OnlineNetwork.ForwardFeed();

                double actionQ = OnlineNetwork.GetOutputs()[memory.Actions[0]];
                double temporalDifference = cumulativeRewards - actionQ;

                memory.SetTemporalDifference(temporalDifference);
                MemoryBuffer.UpdatePriority(memory);

                statistics.RecordLossCalculation(NeuralMath.HuberLoss(temporalDifference));

                for(int i = 0; i < targets.Count; i++)
                    targets[i] += cumulativeRewards;

                output.Add(new TrainingSample(targets, memory.Actions[0], memory.States[0]));
            }

            return output;
        }

        public void Train()
        {
            for (int i = 0; i < 4; i++)
            {               
                collectedInteractionMemories = new List<NeuralMemory>();

                var trainingSamples = GetTrainingSamples(MemoryBuffer.SampleBatch(Configuration.BatchSize));

                foreach (TrainingSample sample in trainingSamples)
                {
                    statistics.RecordQEstimation();
                    
                    HuberLossBackpropagationSpecification specification = new HuberLossBackpropagationSpecification(sample.Action, sample.Targets.Values.First());

                    OnlineNetwork.FillInputs(sample.State.ToDictionary(k => k.Key, v => (double)v.Value));
                    OnlineNetwork.ForwardFeed();
                    OnlineNetwork.Backpropagate(specification);
                    OnlineNetwork.UpdateWeights();
                }
            }            
        }

        private double CalculateReward(decimal positionStartValue, decimal currentPositionValue)
        {
            double reward = (double)((currentPositionValue - positionStartValue)/positionStartValue);
            return reward;
        }

        //private double CalculateReward(decimal positionValue, decimal equityChange, decimal commisionFraction)
        //{
        //    //double valueCoef = Math.Tanh((rewardVariables["Cash"] + rewardVariables["Equity"] - rewardVariables["StartingCash"])
        //    //   / (rewardVariables["StartingCash"]));
        //    //double equityChangeCoef = Math.Tanh(rewardVariables["EquityChange"] / (0.3d * rewardVariables["StartingCash"])) + 1;
        //    //double validityCoef = Math.Min(Math.Tanh(rewardVariables["Validity"] / 2) + 0.55d, 1);

        //    double equityChangeCoef = 0;

        //    //equityChangeCoef = 2 * Math.Atanh((double)((equityChange - Math.Abs(positionValue) * 2 * commisionPercentage)/ (0.3M * Math.Abs(positionValue))));
        //    //equityChangeCoef = (double)(equityChange - Math.Abs(positionValue) * 2 * commisionFraction);
        //    equityChangeCoef = (double)(equityChange);

        //    double reward = equityChangeCoef; //* validityCoef;

        //    return reward;
        //}

        private void ProcessCollectedMemories(double newReward, Dictionary<string, double> newState, 
            List<string> newPossibleActions,
            bool isEpisodeTerminated = false)
        {
            for (int i = 0; i < collectedInteractionMemories.Count; i++)
            {
                if (!collectedInteractionMemories[i].Complete)
                {
                    collectedInteractionMemories[i].PerformTransition(newReward, newState, newPossibleActions);

                    if (isEpisodeTerminated)
                    {
                        collectedInteractionMemories[i].CompleteEpisode();
                        collectedInteractionMemories[i].CompleteMemory();
                    }
                    else if (collectedInteractionMemories[i].TimeStep == Configuration.MemorySteps - 1)
                    {
                        collectedInteractionMemories[i].CompleteMemory();
                    }
                }
            }
        }

        public void ResetStatistics()
        {
            statistics = new LearningAgentStatistics();
        }

        public LearningAgentStatistics GetStatistics()
        {
            return statistics;
        }

        LearningAgentStatistics IStatisticsProvider<LearningAgentStatistics>.GetStatistics()
        {
            return statistics;
        }
    }
}
