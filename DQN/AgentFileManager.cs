using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AlgoTrading.DataKeeping;
using AlgoTrading.Neural;
using Newtonsoft.Json;

namespace AlgoTrading.DQN
{
    public class AgentFileManager : FileManager
    {
        public AgentFileManager(string agentName) : base(agentName + ".json")
        {
            FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GeneticTradeOptimizer\Agents\";
        }

        public void SaveAgent(Agent agent)
        {           
            string jsonString = JsonConvert.SerializeObject(agent, Formatting.Indented);
            List<string> jsonLines = jsonString.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

            WriteToFile(jsonLines);
        }

        public Agent LoadAgent()
        {
            List<string> jsonLines = ReadFile();
            return JsonConvert.DeserializeObject<Agent>(String.Join(Environment.NewLine, jsonLines));
        }
    }
}
