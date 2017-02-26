namespace Kontur.GameStats.Server
{
    public class GetServersMatches : RequestHandler<MatchParameters>
    {
        private readonly IRepository<Model.Match> matchesTable;

        public GetServersMatches(IRepository<Model.Match> matchesTable)
        {
            this.matchesTable = matchesTable;
        }

        public override object Process(MatchParameters parameters, object data)
        {
            Model.MatchResults result = matchesTable
                .GetOne(x => x.Server == parameters.Endpoint && x.Timestamp == parameters.Timestamp)
                .Results;

            if (result == null)
            {
                throw new PageNotFoundException("Match with given endpoint at given timestamp does not exist");
            }

            return result;
        }
    }
}
