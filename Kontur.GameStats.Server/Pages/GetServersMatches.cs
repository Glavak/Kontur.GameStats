using LiteDB;

namespace Kontur.GameStats.Server
{
    public class GetServersMatches : RequestHandler<MatchParameters>
    {
        public override object Process(MatchParameters parameters, object data, LiteDatabase database)
        {
            var table = database.GetCollection<Model.Match>("matches");

            Model.MatchResults result = table
                .FindOne(x => x.Server == parameters.Endpoint && x.Timestamp == parameters.Timestamp)
                .Results;

            if (result == null)
            {
                throw new PageNotFoundException("Match with given endpoint at given timestamp does not exist");
            }

            return result;
        }
    }
}
