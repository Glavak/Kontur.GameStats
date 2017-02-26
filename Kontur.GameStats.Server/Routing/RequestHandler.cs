namespace Kontur.GameStats.Server
{
    /// <summary>
    /// Abstract class, which parses parameters from string to TParameters, and
    /// calls overriden by child method Process()
    /// </summary>
    /// <typeparam name="TParameters">IParameters object, that child request handler require</typeparam>
    public abstract class RequestHandler<TParameters> : IRequestHandler
        where TParameters : IParameters, new()
    {
        public object Handle(string[] parameters, object data)
        {
            var parametersObject = new TParameters();
            parametersObject.SetValues(parameters);

            return Process(parametersObject, data);
        }

        public abstract object Process(TParameters parameters, object data);
    }
}
