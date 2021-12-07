using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.Threading;
using System.Diagnostics;

using AlgoTrading.TradeOptimizer.Interfaces;
using AlgoTrading.TradeOptimizer.IndexManagers;

namespace AlgoTrading.TradeOptimizer.Backtesters
{
    public class PythonScriptBacktester : IStrategyBacktester
    {
        public static int BacktesterID { get; private set; } = 0;
        public string ScriptPath { get; private set; }
        public string ScriptName { get; set; }
        public Process PythonScriptProcess { get; private set; }
        public JsonScriptFileManager IndexManager { get; private set; }
        public PythonCommunicationManager CommunicationManager { get; private set; }

        
        public PythonScriptBacktester(string pythonScriptName, string indexFileName = "")
        {
            BacktesterID++;
            ScriptName = pythonScriptName;
            ScriptPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GeneticTradeOptimizer\Scripts\" + ScriptName;

            if (!File.Exists(ScriptPath))
                throw (new FileNotFoundException("Script file not found"));

            if (indexFileName == String.Empty)
                indexFileName = Path.GetFileNameWithoutExtension(pythonScriptName) + "-indexes.json";

            IndexManager = new JsonScriptFileManager(indexFileName);

            ProcessStartInfo scriptStartInfo = new ProcessStartInfo(ScriptPath);
            scriptStartInfo.Arguments = $"{BacktesterID}";
            scriptStartInfo.UseShellExecute = true;
            //scriptStartInfo.CreateNoWindow = true;
            PythonScriptProcess = Process.Start(scriptStartInfo);

            CommunicationManager = new PythonCommunicationManager(BacktesterID);
        }



        public Dictionary<string, float> RunBacktest(Dictionary<string, float> testIndexes)
        {
            CommunicationManager.ClearSharedMemory();

            string output = IndexManager.ChangeIndexes(testIndexes, false);
            CommunicationManager.WriteToSharedMemory(output);
            CommunicationManager.WriteToPipe("receiveValues" + " " + output.Length);

            if (CommunicationManager.ReadFromPipe() != "done")
                throw new ArgumentException();
            
            CommunicationManager.WriteToPipe("runTest");

            string results = CommunicationManager.ReadFromPipe();

            if (results == "error")
                throw new ArgumentException();

            Dictionary<string, float> outDic = IndexManager.DeserializeIndexes(results);

            for(int i = 0; i < outDic.Count(); i++)
            {
                float resultValue = outDic.ElementAt(i).Value;
                if (double.IsNaN(resultValue))
                    throw new ArgumentNullException();
            }

            return outDic;
        }

        public Dictionary<string, float> GetStrategyIndexes()
        {
            return IndexManager.GetFileIndexes();
        }
    }
}
