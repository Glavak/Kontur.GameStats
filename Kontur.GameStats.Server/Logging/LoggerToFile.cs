using System;
using System.IO;

namespace Kontur.GameStats.Server
{
    public class LoggerToFile : ILogger, IDisposable
    {
        private readonly StreamWriter writer;
        private bool disposed;

        public LoggerToFile(string filename)
        {
            writer = new StreamWriter(filename);
        }

        public void Log(MessageType messageType, string message)
        {
            string messageTypeString;

            switch (messageType)
            {
                    case MessageType.Error:
                    messageTypeString = "ERROR";
                    break;
                case MessageType.Warning:
                    messageTypeString = "WARNING";
                    break;
                case MessageType.Info:
                    messageTypeString = "INFO";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null);
            }

            writer.WriteLine("[{0}] {1} {2}", messageTypeString, DateTime.Now, message);
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
