using AlgoTrading.Broker;
using AlgoTrading.Neural;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTrading.Agent
{
    public abstract class TradingAgent : IAgent
    {
        public IBroker Broker { get; private set; }

        public NeuralNetwork OnlineNetwork { get; private set; }

        public TradingAgent(NeuralNetwork network, IBroker broker)
        {
            OnlineNetwork = network;
            Broker = broker;
        }

        public virtual async Task<bool> Interact()
        {
            MarketState currentState = await Broker.GetNextTimestep();
            List<BrokerAction> possibleActions = Broker.GetAvailableActions();

            if (possibleActions.Count > 0)
            {
                await EmulateAction(SelectActionFromState(currentState, possibleActions));

                return true;
            }

            return false;
        }

        protected virtual async Task EmulateAction(BrokerAction action)
        {
            if (action != BrokerAction.Skip && action != BrokerAction.Close)
            {
                int newPositionSize = 0;
                switch (action)
                {
                    case BrokerAction.Long25:
                        newPositionSize = Broker.GetLimitedPositionSize(0.25d);
                        break;
                    case BrokerAction.Long50:
                        newPositionSize = Broker.GetLimitedPositionSize(0.5d);
                        break;
                    case BrokerAction.Long75:
                        newPositionSize = Broker.GetLimitedPositionSize(0.75d);
                        break;
                    case BrokerAction.Short25:
                        newPositionSize = -1 * Broker.GetLimitedPositionSize(0.25d);
                        break;
                    case BrokerAction.Short50:
                        newPositionSize = -1 * Broker.GetLimitedPositionSize(0.5d);
                        break;
                    case BrokerAction.Short75:
                        newPositionSize = -1 * Broker.GetLimitedPositionSize(0.75d);
                        break;
                }

                Broker.EnterPosition(newPositionSize);
            }
            else
            {
                switch (action)
                {
                    case BrokerAction.Close:
                        Broker.ExitPosition();
                        break;
                }
            }
        }

        protected virtual BrokerAction SelectActionFromState(MarketState marketState, List<BrokerAction> possibleActions)
        {
            OnlineNetwork.FillInputs(marketState.ToDictionary());
            OnlineNetwork.ForwardFeed();
            Dictionary<string, double> outputs = OnlineNetwork.GetOutputs();

            KeyValuePair<BrokerAction, double> actionValue = GetMaxQAction(outputs, possibleActions);

            return actionValue.Key;
        }

        protected KeyValuePair<BrokerAction, double> GetMaxQAction(Dictionary<string, double> actionValues, List<BrokerAction> possibleActions)
        {
            double maxQ = double.MinValue;
            BrokerAction action = BrokerAction.Skip;

            foreach (BrokerAction possibleAction in possibleActions)
            {
                string actionString = possibleAction.GetActionString();

                if (actionValues[actionString] > maxQ)
                {
                    maxQ = actionValues[actionString];
                    action = possibleAction;
                }
            }

            return new KeyValuePair<BrokerAction, double>(action, maxQ);
        }
    }
}
