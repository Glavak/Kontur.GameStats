﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
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

        /// <summary>
        /// Finds appropriate requestHandler for given adress and httpMethod, parses
        /// adress to parameters and calls requestHandler.Handle
        /// </summary>
        /// <param name="address"></param>
        /// <param name="data">Data, which will be passed to requestHandler.Handle</param>
        /// <param name="httpMethod"></param>
        /// <returns>Data returned from requestHandler</returns>
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

            unityContainer.Resolve<ILogger>().Log(MessageType.Info,
                "User attempted to open not existing page, url: \"" + 
                address + 
                "\". 404 returned.");

            throw new PageNotFoundException("");
        }
    }
}
