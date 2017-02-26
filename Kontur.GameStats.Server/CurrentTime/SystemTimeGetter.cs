using System;

namespace Kontur.GameStats.Server
{
    public class SystemTimeGetter : ICurrentTimeGetter
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}
