namespace Kontur.GameStats.Server
{
    public class PlayerNameParameters : IParameters
    {
        [ParameterString(Required = true)]
        public string Name { get; set; }
    }
}
