using System;

namespace Kontur.GameStats.Server
{
    public class SystemTimeGetter : ICurrentTimeGetter
    {
        {
            return DateTime.Now;
        }
    }
}
