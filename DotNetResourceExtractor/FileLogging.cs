using System;
using System.Diagnostics;
using System.IO;
using ChuckHill2;

namespace DotNetResourceExtractor
{
    [Serializable]
    public class FileLogging: MarshalByRefObject, IDisposable
    {
        private StreamWriter stream;
        private string OutputFile = "DebugLog.txt";
        private string OutputFileTemp = "DebugLog.temp";

        public FileLogging(string outputFile = null)
        {
            if (!string.IsNullOrEmpty(outputFile))
            {
                OutputFile = outputFile;
                OutputFileTemp = Path.ChangeExtension(outputFile, ".temp");
            }

            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_DomainUnload;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_DomainUnload;

            stream = new StreamWriter(OutputFileTemp) { AutoFlush = true };
            stream.WriteLine($"==== {Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName)} Debug Log {DateTime.Now.ToString("g")} ====");
            stream.WriteLine();
        }

        private void CurrentDomain_DomainUnload(object sender, EventArgs e) => Dispose();
        public void Dispose()
        {
            if (stream == null) return;
            AppDomain.CurrentDomain.DomainUnload -= CurrentDomain_DomainUnload;
            AppDomain.CurrentDomain.ProcessExit -= CurrentDomain_DomainUnload;
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_DomainUnload;
            stream.Close();
            FileEx.Move(OutputFileTemp, OutputFile);
            stream = null;
        }

        public void WriteLine(string msg)
        {
            lock (stream)
            {
                stream.WriteLine(msg);
            }
        }
    }
}
