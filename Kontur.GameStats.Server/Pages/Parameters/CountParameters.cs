namespace Kontur.GameStats.Server
{
    public class CountParameters : IParameters
    {
        public const int MaximumCountValue = 50;

        [ParameterInteger(MinValue = 0, MaxValue = MaximumCountValue, Required = false, DefaultValue = 5)]
        public int Count { get; set; }
    }
}
