using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server
{
    public class ReportPopularServers : RequestHandler<CountParameters>
    {
        private IRepository<Model.Server> serversTable;

        public ReportPopularServers(IRepository<Model.Server> serversTable)
        {
            this.serversTable = serversTable;
        }

        private Model.PopularServersServer[] popularServersCache;
        private DateTime lasTimeRecached;

        public override object Process(CountParameters parameters, object data)
        {
            if (popularServersCache == null || (DateTime.Now - lasTimeRecached).TotalMinutes > 1)
            {
                // Need to recache

                var servers = serversTable
                    .GetSorted("AverageMatchesPerDay", Query.Descending, CountParameters.MaximumCountValue)
                    .ToArray();

                popularServersCache = servers
                    .Select(x => new Model.PopularServersServer(x))
                    .ToArray();

                lasTimeRecached = DateTime.Now;
            }

            return popularServersCache.Take(parameters.Count);
        }
    }
}
