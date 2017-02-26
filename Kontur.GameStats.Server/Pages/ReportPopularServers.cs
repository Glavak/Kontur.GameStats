using System.Collections.Generic;
using LiteDB;
using System.Linq;

namespace Kontur.GameStats.Server
{
    public class ReportPopularServers : CachedRequestHandler<Model.PopularServersServer>
    {
        private readonly IRepository<Model.Server> serversTable;

        public ReportPopularServers(IRepository<Model.Server> serversTable, ICurrentTimeGetter timeGetter) : base(timeGetter)
        {
            this.serversTable = serversTable;
        }

        public override IEnumerable<Model.PopularServersServer> Recache()
        {
            var servers = serversTable
                .GetSorted("AverageMatchesPerDay", Query.Descending, CountParameters.MaximumCountValue);

            return servers
                .Select(x => new Model.PopularServersServer(x));
        }
    }
}
