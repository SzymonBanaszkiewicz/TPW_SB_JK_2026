using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;

namespace Data
{
    public static class DiagnosticLogger
    {
        private static readonly BlockingCollection<string> _logQueue =
            new(new ConcurrentQueue<string>());

        private static readonly string _filePath = "simulation_diagnostic.log";

        private static readonly Thread _loggingThread;

        private static volatile bool _isRunning = true;

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
            if (!_isRunning) return;

            try
            {
                _logQueue.Add(message);
            }
            catch (InvalidOperationException)
            {
                // logger zamknięty
            }
        }

        private static void ProcessQueue()
        {
            using var writer = new StreamWriter(_filePath, true, Encoding.UTF8);

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

            writer.Flush();
        }


        public static void Shutdown()
        {
            _isRunning = false;
            _logQueue.CompleteAdding();
            _loggingThread.Join();
        }
    }
}