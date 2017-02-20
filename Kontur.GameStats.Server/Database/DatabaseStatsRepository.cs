using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server
{
    public class DatabaseStatsRepository : IStatsRepository
    {
        private readonly StatsDbContext context;

        public DatabaseStatsRepository(StatsDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Model.servers> GetServers()
        {
            return context.Servers;
        }
    }
}
