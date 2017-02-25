using System.Collections.Generic;
using System.Text.RegularExpressions;
using LiteDB;
using Microsoft.Practices.Unity;

namespace Kontur.GameStats.Server
{
    public class Router
    {
        private readonly List<RouterBinding> bindings;
        private readonly UnityContainer unityContainer;

        public Router(UnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;

            bindings = new List<RouterBinding>();
        }

        public void Bind<THandler>(string addressRegex, string httpMethod)
            where THandler : IRequestHandler
        {
            bindings.Add(new RouterBinding(addressRegex, httpMethod, typeof(THandler)));
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

                var handler = (IRequestHandler)unityContainer.Resolve(bind.RequestHandlerType);
                return handler.Handle(parameters, data);
            }

            throw new PageNotFoundException("");
        }
    }
}
