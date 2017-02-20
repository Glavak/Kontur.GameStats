namespace Kontur.GameStats.Server
{
    public class RouterBinding
    {
        public string AddressRegexp { get; }
        public string HttpMethod { get; }
        public IRequestHandler RequestHandler { get; }

        public RouterBinding(string addressRegexp, string httpMethod, IRequestHandler requestHandler)
        {
            AddressRegexp = addressRegexp;
            HttpMethod = httpMethod;
            RequestHandler = requestHandler;
        }
    }
}
