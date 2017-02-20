namespace Kontur.GameStats.Server
{
    public class CountParameters : IParameters
    {
        [ParameterInteger(MinValue = 0, MaxValue = 50, Required = false, DefaultValue = 5)]
        public int Count { get; set; }
    }
}
