using System;
using System.Collections.Generic;
using System.Text;
using AlgoTrading.DataKeeping;
using Newtonsoft.Json;
using System.Linq;
using System.IO;

namespace AlgoTrading.DQN
{
    public class DQNMetaFileManager : FileManager
    {
        public DQNMeta Meta { get; private set; }
        public DQNMetaFileManager(DQNMeta dQNMeta) : base(dQNMeta.Name + ".json")
        {
            FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GeneticTradeOptimizer\DQN\";
            Meta = dQNMeta;
        }

        public DQNMetaFileManager(string dqnMetaFileName) : base(dqnMetaFileName)
        {
            FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GeneticTradeOptimizer\DQN\";
            Meta = LoadMeta();
        }

        public void SaveMeta()
        {
            string jsonString = JsonConvert.SerializeObject(Meta, Formatting.Indented);
            List<string> jsonLines = jsonString.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

            WriteToFile(jsonLines);
        }

        public DQNMeta LoadMeta()
        {
            List<string> jsonLines = ReadFile();
            return Meta = JsonConvert.DeserializeObject<DQNMeta>(String.Join(Environment.NewLine, jsonLines));
        }

        public static List<string> GetAvailableMeta()
        {
            List<string> filePaths = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GeneticTradeOptimizer\DQN").ToList();
            List<string> output = new List<string>();
            foreach (string path in filePaths)
            {
                output.Add(Path.GetFileNameWithoutExtension(path));
            }

            return output;
        }
    }
}
