using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.Diagnostics;

namespace AlgoTrading.TradeOptimizer.Backtesters
{
    public class PythonCommunicationManager
    {
        public NamedPipeServerStream ServerPipeStream { get; private set; }
        public MemoryMappedFile SharedMemoryFile { get; private set; }
        public MemoryMappedViewStream SharedFileStream { get; private set; }
        public int CommunicationID { get; private set; }

        public PythonCommunicationManager(int communicationID)
        {
            CommunicationID = communicationID;

            Console.WriteLine("Piping...");
            CreatePipeConnection();           
        }

        async void CreatePipeConnection()
        {
            ServerPipeStream = new NamedPipeServerStream($"NETtoPython{CommunicationID}", PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.None, 65536, 65536);
            SharedMemoryFile = MemoryMappedFile.CreateOrOpen($"SharedNETPython{CommunicationID}", 10000, MemoryMappedFileAccess.ReadWrite);

            await Task.Run(() =>
            {
                ServerPipeStream.WaitForConnection();
                Console.WriteLine($"Pipe ID {CommunicationID} established.");

                SharedFileStream = SharedMemoryFile.CreateViewStream();
            });
        }

        public void WriteToSharedMemory(string content)
        {
            byte[] contentBytes = Encoding.UTF8.GetBytes(content);
            byte[] buffer = new byte[2048];

            int bytesLeft = contentBytes.Length;
            while (bytesLeft > 0)
            {
                Array.Clear(buffer, 0, buffer.Length);
                Array.Copy(contentBytes, contentBytes.Length - bytesLeft, buffer, 0, Math.Min(bytesLeft, buffer.Length));

                SharedFileStream.Write(buffer, 0, Math.Min(bytesLeft, buffer.Length));
                bytesLeft -= Math.Min(bytesLeft, buffer.Length);

            }

        }

        public string ReadFromSharedMemory()
        {
            byte[] buffer = new byte[2048];
            string output = String.Empty;
            int bytesRead = -1;

            while ((bytesRead = SharedFileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                if (bytesRead == -1)
                    Thread.Sleep(500);
                else
                {
                    output += Encoding.UTF8.GetString(buffer);
                    Array.Clear(buffer, 0, buffer.Length);
                }
            }

            output = output.Substring(0, output.IndexOf('\0'));
            return output.Trim();
        }

        public void ClearSharedMemory()
        {
            SharedFileStream.Seek(0, SeekOrigin.Begin);
            SharedFileStream.Write(new byte[SharedFileStream.Length], 0, Convert.ToInt32(SharedFileStream.Length));
            SharedFileStream.Flush();
            SharedFileStream.Seek(0, SeekOrigin.Begin);
        }

        public void WriteToPipe(string content)
        {
            byte[] contentBytes = Encoding.UTF8.GetBytes(content);
            byte[] buffer = new byte[2048];

            int bytesLeft = contentBytes.Length;
            while(bytesLeft > 0)
            {
                Array.Copy(contentBytes, buffer, Math.Min(buffer.Length, contentBytes.Length));

                ServerPipeStream.Write(buffer, contentBytes.Length - bytesLeft, Math.Min(bytesLeft, buffer.Length));
                bytesLeft -= Math.Min(bytesLeft, buffer.Length);
            }          
        }

        public string ReadFromPipe()
        {
            ServerPipeStream.WaitForPipeDrain();

            byte[] buffer = new byte[2048];
            string output = String.Empty;
            int bytesRead = -1;

            while((bytesRead = ServerPipeStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                if (bytesRead == -1)
                    Thread.Sleep(500);
                else
                {
                    output += Encoding.UTF8.GetString(buffer);
                    Array.Clear(buffer, 0, buffer.Length);

                    if (ServerPipeStream.IsMessageComplete)
                    {
                        output = output.Substring(0, output.IndexOf('\0'));
                        break;
                    }
                }              
            }
            
            return output.Trim();
        }
    }
}
