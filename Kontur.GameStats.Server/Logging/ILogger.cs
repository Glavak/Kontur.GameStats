namespace Kontur.GameStats.Server
{
    public interface ILogger
    {
        void Log(MessageType messageType, string message);
    }
}
