using System.Collections.Generic;
using System.Text.RegularExpressions;
using LiteDB;

namespace Kontur.GameStats.Server
{
    public class Router
    {
        private readonly List<RouterBinding> bindings;
        private readonly LiteDatabase database;

        public Router(LiteDatabase database)
        {
            this.database = database;

            bindings = new List<RouterBinding>();
        }

        public void Bind(string addressRegex, string httpMethod, IRequestHandler handler)
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
                return bind.RequestHandler.Handle(parameters, data, database);
            }

            throw new PageNotFoundException("");
        }
    }
}
