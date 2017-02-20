using System;

namespace Kontur.GameStats.Server
{
    public class MatchParameters : IParameters
    {
        // Regexp in Router binds should always set Endpoint to be "Host-Port"

        [ParameterString(Required = true)]
        public string Endpoint { get; set; }

        [ParameterString(Required = true)]
        public string Host { get; set; }

        [ParameterInteger(Required = true)]
        public int Port { get; set; }

        [ParameterTimestamp(Required = true)]
        public DateTime Timestamp { get; set; }
    }
}
