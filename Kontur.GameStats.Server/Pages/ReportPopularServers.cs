using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server
{
    public class ReportPopularServers : CachedRequestHandler<Model.PopularServersServer>
    {
        private IRepository<Model.Server> serversTable;

        public ReportPopularServers(IRepository<Model.Server> serversTable)
        {
            this.serversTable = serversTable;
        }

        public override Model.PopularServersServer[] Recache()
        {
            var servers = serversTable
                .GetSorted("AverageMatchesPerDay", Query.Descending, CountParameters.MaximumCountValue);

            return servers
                .Select(x => new Model.PopularServersServer(x))
                .ToArray();
        }
    }
}
