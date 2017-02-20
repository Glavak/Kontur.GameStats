using System.Collections.Generic;

namespace Kontur.GameStats.Server
{
    public interface IStatsRepository
    {
        IEnumerable<Model.servers> GetServers();
    }
}
