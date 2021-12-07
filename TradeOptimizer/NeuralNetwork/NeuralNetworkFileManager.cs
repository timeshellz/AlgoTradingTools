using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AlgoTrading.DataKeeping;
using Newtonsoft.Json;

namespace AlgoTrading.Neural
{
    public class NeuralNetworkFileManager : FileManager
    {
        JsonSerializerSettings serializerSettings;
        public NeuralNetworkFileManager(string networkName) : base(networkName + ".json")
        {
            FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GeneticTradeOptimizer\NeuralNetworks\";
            serializerSettings = new JsonSerializerSettings();
            //serializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
        }

        public void SaveNetwork(NeuralNetwork network)
        {
            string jsonString = JsonConvert.SerializeObject(network, Formatting.Indented, serializerSettings);
            List<string> jsonLines = jsonString.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

            WriteToFile(jsonLines);
        }

        public NeuralNetwork LoadNetwork()
        {
            List<string> jsonLines = ReadFile();
            return JsonConvert.DeserializeObject<NeuralNetwork>(String.Join(Environment.NewLine, jsonLines), serializerSettings);
        }
    }
}
