namespace Kontur.GameStats.Server
{
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
