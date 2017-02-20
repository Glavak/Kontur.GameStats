using Kontur.GameStats.Server.Model;
using LiteDB;

namespace Kontur.GameStats.Server
{
    public class GetServersInfo : RequestHandler<CountParameters>
    {
        public override object Process(CountParameters parameters, object data, LiteDatabase database)
        {
            var table = database.GetCollection<Model.Server>("servers");

            return table.FindAll();
        }
    }
}
