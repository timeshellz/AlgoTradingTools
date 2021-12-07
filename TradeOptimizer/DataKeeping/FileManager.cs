using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTrading.DataKeeping
{
    public class FileManager : IFileManager
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public FileManager(string fileName)
        {
            FileName = fileName;

            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GeneticTradeOptimizer\");
            FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GeneticTradeOptimizer\";
        }

        public List<string> ReadFile()
        {
            List<string> output;

            using (FileStream stream = new FileStream(FilePath + FileName, FileMode.Open))
            {
                StreamReader reader = new StreamReader(stream);
                output = new List<string>();

                while (!reader.EndOfStream)
                {
                    output.Add(reader.ReadLine());
                }
            }

            return output;
        }

        public void WriteToFile(List<string> input)
        {
            File.WriteAllText(FilePath + FileName, String.Empty);

            using (FileStream stream = new FileStream(FilePath + FileName, FileMode.OpenOrCreate))
            {                
                StreamWriter writer = new StreamWriter(stream);

                foreach (string line in input)
                    writer.WriteLine(line);

                writer.Close();
            }
        }

        public void ClearFolder()
        {
            string[] files = Directory.GetFiles(FilePath);

            if (files.Length == 0)
                return;

            foreach(string file in files)
            {
                File.Delete(file);
            }
        }
    }

    interface IFileManager
    {
        string FileName { get; set; }
        string FilePath { get; set; }

        List<string> ReadFile();

        void WriteToFile(List<string> input);
    }
}
