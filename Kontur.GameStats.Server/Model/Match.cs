using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace Kontur.GameStats.Server.Model
{
    public class Match
    {
        public string Endpoint { get; set; }

        public DateTime Timestamp { get; set; }

        public MatchResults Results { get; set; }
    }
}
