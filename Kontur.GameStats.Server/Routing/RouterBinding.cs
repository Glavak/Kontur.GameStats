using System;
namespace Kontur.GameStats.Server
{
    public class RouterBinding
    {
        public string AddressRegexp { get; private set; }
        public string HttpMethod { get; private set; }
        public Type RequestHandlerType { get; private set; }

        public RouterBinding(string addressRegexp, string httpMethod, Type requestHandlerType)
        {
            AddressRegexp = addressRegexp;
            HttpMethod = httpMethod;
            RequestHandlerType = requestHandlerType;
        }
    }
}
