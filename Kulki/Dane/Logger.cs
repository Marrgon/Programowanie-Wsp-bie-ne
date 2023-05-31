using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TPW.Dane
{
    public class Logger : IDisposable
    {
        private BlockingCollection<Ball> FirstInFirstOut;
        private StreamWriter streamWriter;
        private string filename = "Logger.json";
        private bool isFlushing = false;
        private Timer timer;

        private readonly ConcurrentQueue<string> logQueue;

        private readonly object lockObj = new object(); // Obiekt blokujący

        private void EndlessLoop()
        {
            try
            {
                foreach (Ball ball in FirstInFirstOut.GetConsumingEnumerable())
                {
                    string json = JsonConvert.SerializeObject(ball);
                    streamWriter.WriteLine($"{DateTime.Now}: {json}");
                }
            }
            finally
            {
                Dispose();
            }
        }

        public Logger()
        {
            if (!File.Exists(filename))
            {
                streamWriter = File.CreateText(filename);
            }
            else
            {
                streamWriter = new StreamWriter(filename, true);
            }

            FirstInFirstOut = new BlockingCollection<Ball>();

            Task.Run(EndlessLoop);

            logQueue = new ConcurrentQueue<string>();

            // Ustawienie timera na wywoływanie metody FlushQueue() co 1 sekundę
            timer = new Timer(FlushQueue, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        public void Log(Ball ball)
        {
            string json = JsonConvert.SerializeObject(ball);
            string logEntry = $"{DateTime.Now}: {json}";
            logQueue.Enqueue(logEntry);
        }

        private void FlushQueue(object state)
        {
            if (!isFlushing)
            {
                isFlushing = true;

                while (logQueue.TryDequeue(out string logEntry))
                {
                    streamWriter.WriteLine(logEntry);
                }

                streamWriter.Flush();

                isFlushing = false;
            }
        }

        public void Dispose()
        {
            timer.Dispose();
            streamWriter.Dispose();
            FirstInFirstOut.Dispose();
        }
    }
}

