using System.Linq;
using Kontur.GameStats.Server.Model;
using LiteDB;

namespace Kontur.GameStats.Server
{
    public class ReportRecentMatches : CachedRequestHandler<Match>
    {
        private readonly IRepository<Match> matchesTable;

        public ReportRecentMatches(IRepository<Match> matchesTable, ICurrentTimeGetter timeGetter) : base(timeGetter)
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
