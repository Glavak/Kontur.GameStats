namespace Kontur.GameStats.Server
{
    public class RouterBinding
    {
        public string AddressRegexp { get; private set; }
        public string HttpMethod { get; private set; }
        public IRequestHandler RequestHandler { get; private set; }

        public RouterBinding(string addressRegexp, string httpMethod, IRequestHandler requestHandler)
        {
            AddressRegexp = addressRegexp;
            HttpMethod = httpMethod;
            RequestHandler = requestHandler;
        }
    }
}
