namespace Kontur.GameStats.Server
{
    public class PlayerNameParameters : IParameters
    {
        [ParameterString(Required = true, Lowercase = true)]
        public string Name { get; set; }
    }
}
