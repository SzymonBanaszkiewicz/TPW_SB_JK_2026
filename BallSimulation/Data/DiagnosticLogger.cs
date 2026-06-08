using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Data
{
    public static class DiagnosticLogger
    {
        private static readonly BlockingCollection<string> _logQueue = new BlockingCollection<string>(new ConcurrentQueue<string>());
        private static readonly string _filePath = "simulation_diagnostic.log";
        private static readonly Thread _loggingThread;

        static DiagnosticLogger()
        {
            _loggingThread = new Thread(ProcessQueue)
            {
                IsBackground = true,
                Name = "DiagnosticLoggerThread"
            };
            _loggingThread.Start();
        }

        public static void QueueLog(string message)
        {
            if (!_logQueue.IsAddingCompleted)
            {
                _logQueue.Add(message);
            }
        }

        private static void ProcessQueue()
        {
            using (StreamWriter writer = new StreamWriter(_filePath, true, Encoding.ASCII))
            {
                foreach (var logLine in _logQueue.GetConsumingEnumerable())
                {
                    try
                    {
                        writer.WriteLine(logLine);
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(10);
                    }
                }
            }
        }

        public static void Stop()
        {
            _logQueue.CompleteAdding();
        }
    }
}