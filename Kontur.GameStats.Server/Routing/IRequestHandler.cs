namespace Kontur.GameStats.Server
{
    public interface IRequestHandler
    {
        /// <summary>
        /// Handles request to server
        /// </summary>
        /// <param name="parameters">Url parameters of request</param>
        /// <param name="data">Parsed body of request</param>
        /// <returns>Object, representing result of the request. It can be then encoded to JSON/XML to return to client</returns>
        object Handle(string[] parameters, object data);
    }
}
