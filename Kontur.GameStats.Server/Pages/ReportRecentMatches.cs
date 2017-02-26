using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontur.GameStats.Server.Model;
using LiteDB;

namespace Kontur.GameStats.Server
{
    public class ReportRecentMatches : CachedRequestHandler<Model.Match>
    {
        private IRepository<Model.Match> matchesTable;

        public ReportRecentMatches(IRepository<Model.Match> matchesTable)
        {
            this.matchesTable = matchesTable;
        }

        public override Match[] Recache()
        {
            return matchesTable
                    .GetSorted("Timestamp", Query.Descending, CountParameters.MaximumCountValue)
                    .ToArray();
        }
    }
}
