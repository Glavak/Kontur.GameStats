using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Model
{
    public class MatchResults
    {
        public string Map { get; set; }
        public string GameMode { get; set; }
        public int FragLimit { get; set; }
        public int TimeLimit { get; set; }
        public double TimeElapsed { get; set; }
        public List<PlayerScore> Scoreboard { get; set; }

    }
}
