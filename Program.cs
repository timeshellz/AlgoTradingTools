using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using AlgoTrading.TradeOptimizer.IndexManagers;
using AlgoTrading.DataKeeping;


namespace AlgoTrading.TradeOptimizer
{
    class Program
    {
        public static Dictionary<string, Action> Methods { get; set; }
        public static GeneticController PrimaryController;


        static List<string> controllerArguments;

        [STAThread]
        static void Main(string[] args)
        {
            Methods = new Dictionary<string, Action>()
            {
                ["help"] = DrawHelp,
                ["gencreate"] = CreateGeneration,
                ["bootcon"] = BootController,                
                ["rungen"] = RunGeneticAlgorithm,
                ["createindexes"] = CreateIndexFile,
                ["setindex"] = SetIndexFileName,
                ["stop"] = () =>
                {
                    return;
                },
            };

            controllerArguments = new List<string>();

            Console.Clear();
            Console.WriteLine("Genetic Trade Optimizer\n\nDeveloped by timeshellz - 2021\n---------------------------------------");

            while (true)
            {
                string command = Console.ReadLine();

                if (Methods.ContainsKey(command))
                    Methods[command]();
                else
                    Console.WriteLine("\nUnknown command.\n");
            }
        }

        static void DrawHelp()
        {
            Console.WriteLine("Help: ");

            foreach(KeyValuePair<string, Action> command in Methods)
            {
                Console.WriteLine(command.Key + " - " + command.Value.Method.Name);
            }
        }

        static void BootController()
        {
            string scriptPath = String.Empty;

            Console.WriteLine("Script name: ");
            scriptPath = Console.ReadLine();

            try
            {
                PrimaryController = new GeneticController(scriptPath, GeneticController.BacktestMode.PythonBacktest, controllerArguments.ToArray());
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("\n" + e.Message);

                if (!String.IsNullOrEmpty(e.FileName))
                    Console.WriteLine(e.FileName);

                Console.WriteLine("\n");

                return;
            }
            catch
            {
                Console.WriteLine("\nController failed to initialize.\n");
                return;
            }

            Console.WriteLine("Script set. Primary controller created successfully.");
            Console.WriteLine("Current controller mode: " + PrimaryController.ControllerMode.ToString());
        }

        static void CreateGeneration()
        {
            if (PrimaryController == null)
            {
                Console.WriteLine("\nController not booted. Use 'bootcon'.\n");
                return;
            }


            int popCap = 0;
            float similarityIndex = 0;
            float crossoverChance = 1;
            float deviation = 0;
            float selection = 0;

            try
            {
                Console.WriteLine("Population cap: ");
                popCap = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Similarity index: ");
                similarityIndex = (float)Convert.ToDouble(Console.ReadLine());

                Console.WriteLine("Crossover chance: ");
                crossoverChance = (float)Convert.ToDouble(Console.ReadLine());

                Console.WriteLine("Initial gene deviation: ");
                deviation = (float)Convert.ToDouble(Console.ReadLine());

                Console.WriteLine("Selection fraction: ");
                selection = (float)Convert.ToDouble(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("\nError occured. Invalid data.\n");
                return;
            }

            PrimaryController.CreateInitialGenetics(popCap, similarityIndex, crossoverChance, deviation, selection);

            Console.WriteLine("Generation created successfully.\n");
        }

        static void RunGeneticAlgorithm()
        {
            PrimaryController.Run();

            Console.WriteLine("Generation completed.\n");
        }

        static void SetIndexFileName()
        {
            if(PrimaryController != null)
            {
                Console.WriteLine("Controller already booted. Settings can't be changed.");
            }

            Console.WriteLine("Set index file name: ");
            string fileName = Console.ReadLine();

            JsonScriptFileManager indexManager;

            try
            {
                 indexManager = new JsonScriptFileManager(fileName, false);
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine("\n" + e.Message);

                if (!String.IsNullOrEmpty(e.FileName))
                    Console.WriteLine(e.FileName);

                Console.WriteLine("\n");
            }
            catch
            {
                Console.WriteLine("\nError occured\n");
            }

            controllerArguments.Add(fileName);
        }

        static void CreateIndexFile()
        {
            Console.WriteLine("Set index file name: ");
            string indexFileName = Console.ReadLine();

            JsonScriptFileManager indexManager = new JsonScriptFileManager(indexFileName, true);
            
            Dictionary<string, float> indexValues = new Dictionary<string, float>();

            int i = 1;
            while (true)
            {
                
                Console.WriteLine("Input identifier for index " + i + ". '1q' to exit: ");
                string line = Console.ReadLine();

                if (line == "1q")
                    break;

                try
                {
                    indexValues.Add(line, 0.0f);
                    i++;
                }
                catch(ArgumentException e)
                {
                    Console.WriteLine(e.Message + " Try again.");
                }
                catch
                {
                    Console.WriteLine("Error occured. Try again.");
                }

            }

            indexManager.ChangeIndexes(indexValues);

            Console.WriteLine("New index file '" + indexFileName + "' created. Contains " + indexValues.Count() + " indexes.\n");
        }
    }
}
