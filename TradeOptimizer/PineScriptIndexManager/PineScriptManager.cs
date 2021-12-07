using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTrading.TradeOptimizer.Interfaces;
using AlgoTrading.TradeOptimizer.DataKeeping;

namespace AlgoTrading.TradeOptimizer.IndexManagers
{
    public class PineScriptManager : FileManager, IIndexManager
    {
        static List<string> AcceptedIndexNames = new List<string>()
        {
            "ddiIndex",
            "stochBuyLimitIndex",
            "rocDifferenceIndex",
            "lastCloseSpanIndex",
            "lastOpenSpanIndex",
            "stopLossValueIndex",
            "buyStochConfidenceIndex",
            "buyRocCrossConfidenceIndex",
            "buyMacdCrossConfidenceIndex",
            "buyMacdNonCrossConfidenceIndex",
            "buyBBStateConfidenceIndex",
            "buyStochBuySignalConfidenceIndex",
            "buyConfidenceSumIndex",
            "previousBuyConfidenceSumIndex",
            "sellStochConfidenceIndex",
            "sellRocCrossConfidenceIndex",
            "sellMacdCrossConfidenceIndex",
            "sellBBStateConfidenceIndex",
            "sellStochBuySignalConfidenceIndex",
            "sellConfidenceSumIndex",
        };

        public Dictionary<string, int> IndexDefinitionLines { get; set; }

        public PineScriptManager(string scriptName) : base(scriptName)
        {
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GeneticTradeOptimizer\Scripts\");
            FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GeneticTradeOptimizer\";          

            if (!File.Exists(FilePath + FileName))
                throw (new FileNotFoundException("Script file not found."));
        }              

        public Dictionary<string, float> GetFileIndexes()
        {
            List<string> scriptLines = ReadFile();
            Dictionary<string, float> outputIndexes = new Dictionary<string, float>();
            IndexDefinitionLines = new Dictionary<string, int>();

            for (int i = 0; i < scriptLines.Count; i++)
            {
                foreach (string name in AcceptedIndexNames)
                {
                    string indexLine = String.Empty;

                    if (scriptLines[i].Contains(name) && !scriptLines[i].Contains(':') && scriptLines[i].Contains('='))
                    {
                        indexLine = scriptLines[i];

                        indexLine = indexLine.Replace("var", "");

                        string[] indexSplits = indexLine.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                        string indexName = indexSplits[0].Trim();
                        float indexValue = (float)Convert.ToDouble(indexSplits[1].Trim());

                        outputIndexes.Add(indexName, indexValue);
                        IndexDefinitionLines.Add(indexName, i);
                    }
                        
                    if (outputIndexes.Count == AcceptedIndexNames.Count)
                        return outputIndexes;
                }               
            }

            return outputIndexes;
        }

        public string ChangeIndexes(Dictionary<string, float> newIndexes, bool writeToFile = false)
        {
            List<string> scriptLines = ReadFile();

            if (IndexDefinitionLines.Count < AcceptedIndexNames.Count)
                throw new InvalidDataException();

            foreach(KeyValuePair<string, int> indexLine in IndexDefinitionLines)
            {
                if (scriptLines[indexLine.Value].Contains(indexLine.Key) && !scriptLines[indexLine.Value].Contains(':') && scriptLines[indexLine.Value].Contains('='))
                {
                    string[] scriptLineSplits = scriptLines[indexLine.Value].Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

                    scriptLines[indexLine.Value] = scriptLineSplits[0] + "= " + newIndexes[indexLine.Key];
                }
                else
                    throw new InvalidDataException();
            }

            if(writeToFile)
                WriteToFile(scriptLines);

            return String.Join(Environment.NewLine, scriptLines.ToArray());
        }
    }
}
