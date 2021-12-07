using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using AlgoTrading.TradeOptimizer.Interfaces;
using AlgoTrading.DataKeeping;

namespace AlgoTrading.TradeOptimizer.IndexManagers
{
    public class JsonScriptFileManager : FileManager, IIndexManager
    {
        public JsonScriptFileManager(string indexFileName, bool createFile = false) : base(indexFileName)
        {
            FilePath += @"Indexes\";

            if (createFile)
                File.Create(FilePath + FileName).Close();

            if (!File.Exists(FilePath + FileName))
                throw (new FileNotFoundException("Index file not found.", indexFileName));
        }

        public Dictionary<string, int> IndexDefinitionLines { get; set; }

        public string ChangeIndexes(Dictionary<string, float> newIndexes, bool writeToFile = true)
        {
            string rawIndexes = JsonConvert.SerializeObject(newIndexes, Formatting.Indented);
            
            if(writeToFile)
            {
                List<string> fileLines = rawIndexes.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                WriteToFile(fileLines);
            }
                
            return rawIndexes;
        }

        public Dictionary<string, float> DeserializeIndexes(string jsonString)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, float>>(jsonString);
        }

        public Dictionary<string, float> GetFileIndexes()
        {
            List<string> fileLines = ReadFile();

            string rawJsonFile = String.Join(Environment.NewLine, fileLines.ToArray());
            return JsonConvert.DeserializeObject<Dictionary<string, float>>(rawJsonFile);
        }
    }
}
