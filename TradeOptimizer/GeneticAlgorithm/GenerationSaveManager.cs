using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using AlgoTrading.DataKeeping;
using AlgoTrading.TradeOptimizer.Genetics;

namespace AlgoTrading.TradeOptimizer.Genetics
{
    public class GenerationSaveManager : FileManager
    {
        public GeneticAlgorithm Algorithm { get; private set; }
        string defaultScriptName;

        public GenerationSaveManager(string scriptName, GeneticAlgorithm algorithm) : base(scriptName)
        {
            FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GeneticTradeOptimizer\Generations\";

            defaultScriptName = Path.GetFileNameWithoutExtension(FileName);

            Algorithm = algorithm;
        }

        public void SaveLastGeneration()
        {
            string jsonGenData = JsonConvert.SerializeObject(Algorithm.BestIndexes, Formatting.Indented);
            string jsonIndividualsData = JsonConvert.SerializeObject(Algorithm.Individuals, Formatting.Indented);

            List<string> jsonGenDataLines = jsonGenData.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            //List<string> jsonIndDataLines = jsonIndividualsData.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

            FileName = defaultScriptName + "_genData" + Algorithm.GenerationCount + ".json";
            FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GeneticTradeOptimizer\Generations\";

            WriteToFile(jsonGenDataLines);

            //FileName = defaultScriptName + "_genIndividuals" + Algorithm.GenerationCount + ".json";
            //FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GeneticTradeOptimizer\Generations\" + FileName;

           // WriteToFile(jsonIndDataLines);

            FileName = defaultScriptName;
        }
    }
}
