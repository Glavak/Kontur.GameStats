using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server
{
    public class LoggerToFile : ILogger, IDisposable
    {
        public StreamWriter writer;
        private bool disposed;

        public LoggerToFile(string filename)
        {
            writer = new StreamWriter(filename);
        }

        public void Log(MessageType messageType, string message)
        {
            writer.WriteLine("[StatServer {0}] {1}", DateTime.Now, message);
        }

        public void Dispose()
        {
            if (disposed)
                return;
            disposed = true;

            writer.Close();
            writer.Dispose();
        }
    }
}
