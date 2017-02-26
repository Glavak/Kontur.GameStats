using System.Collections.Generic;

namespace Kontur.GameStats.Server.Model
{
    public class MatchResults
    {
        public string Map { get; set; }

        public string GameMode { get; set; }

        public int FragLimit { get; set; }

        public int TimeLimit { get; set; }

        public float TimeElapsed { get; set; }

        public List<PlayerScore> Scoreboard { get; set; }
    }
}
