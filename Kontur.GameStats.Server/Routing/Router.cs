using System.Collections.Generic;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace Kontur.GameStats.Server
{
    public class Router
    {
        private readonly List<RouterBinding> bindings;
        private readonly IStatsRepository sqLiteConnection;

        public Router(IStatsRepository sqLiteConnection)
        {
            this.sqLiteConnection = sqLiteConnection;

            bindings = new List<RouterBinding>();

            const string endpointRegexp = @"(([a-zA-Z]+|\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})-(\d+))";
            const string timestampRegexp = @"(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}Z)";
            const string countRegexp = @"(\d+)";

            Bind("^/servers/" + endpointRegexp + "/info/?$", "PUT", new AdvertiseServerInfo());
            Bind("^/servers/" + endpointRegexp + "/info/?$", "GET", new GetServerInfo());
            Bind("^/servers/info/?$", "GET", new GetServersInfo());
            Bind("^/servers/" + endpointRegexp + "/matches/" + timestampRegexp + "/?$", "GET", new GetServersMatches());
        }

        private void Bind(string addressRegex, string httpMethod, IRequestHandler handler)
        {
            bindings.Add(new RouterBinding(addressRegex, httpMethod, handler));
        }

        public object RouteRequest(string address, object data, string httpMethod)
        {
            foreach (var bind in bindings)
            {
                if (httpMethod != bind.HttpMethod) continue;

                Match match = Regex.Match(address, bind.AddressRegexp);
                if (!match.Success) continue;

                string[] parameters = new string[match.Groups.Count - 1];
                // Start with i = 1 to skip the first group, which contains entire string
                for (int i = 1; i < match.Groups.Count; i++)
                {
                    parameters[i - 1] = match.Groups[i].Value;
                }
                return bind.RequestHandler.Handle(parameters, data, sqLiteConnection);
            }

            throw new PageNotFoundException("");
        }
    }
}
