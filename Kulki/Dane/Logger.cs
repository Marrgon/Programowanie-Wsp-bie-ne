using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TPW.Dane
{
    public class Logger : IDisposable
    {
        BlockingCollection<Ball> FirstInFirstOut;
        StreamWriter streamWriter;
        string filename = "Logger.json";

        private void endlessLoop()
        {
            try
            {
                foreach (Ball ball in FirstInFirstOut.GetConsumingEnumerable())
                {
                    string json = JsonConvert.SerializeObject(ball);
                    streamWriter.WriteLine(json);
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
            Task.Run(endlessLoop);
        }

        public void Log(Ball ball) => FirstInFirstOut.Add(ball);

        public void Dispose()
        {
            streamWriter.Dispose();
            FirstInFirstOut.Dispose();
        }
    }
}
