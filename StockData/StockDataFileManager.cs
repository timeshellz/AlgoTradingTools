using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq;
using System.IO;

using AlgoTrading.DataKeeping;

namespace AlgoTrading.Stocks
{
    class StockDataFileManager : FileManager
    {
        public StockData StockData { get; private set; }
        public JsonSerializerSettings Settings { get; private set; }
        IsoDateTimeConverter IsoDateTimeConverter = new IsoDateTimeConverter();

        public StockDataFileManager(string stockDataNameDescription) : base(stockDataNameDescription + ".json")
        {
            FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GeneticTradeOptimizer\StockData\";
        }

        public StockDataFileManager(StockData stockData) : base($"{stockData.Name}@{StockDataController.ConvertIntervalToString(stockData.Interval)}@{stockData.StartDate.ToString("MM-dd-yyyy")}.json")
        {
            StockData = stockData;
            FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GeneticTradeOptimizer\StockData\";
        }

        public void SaveStockData()
        {
            string jsonString = JsonConvert.SerializeObject(StockData, Formatting.Indented, IsoDateTimeConverter);

            List<string> jsonLines = jsonString.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

            WriteToFile(jsonLines);
        }

        public StockData LoadStockData()
        {
            List<string> jsonLines = ReadFile();

            string jsonString = String.Join(Environment.NewLine, jsonLines);

            StockData stockData = JsonConvert.DeserializeObject<StockData>(jsonString, IsoDateTimeConverter);

            StockData = stockData;
            return stockData;
        }

        public static List<string> GetAvailableStockData()
        {
            List<string> filePaths = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GeneticTradeOptimizer\StockData").ToList();
            List<string> output = new List<string>();
            foreach(string path in filePaths)
            {
                output.Add(Path.GetFileNameWithoutExtension(path));
            }

            return output;
        }
    }
}
