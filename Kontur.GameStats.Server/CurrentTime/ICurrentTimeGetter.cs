using System;

namespace Kontur.GameStats.Server
{
    public interface ICurrentTimeGetter
    {
        DateTime GetCurrentTime();
    }
}
