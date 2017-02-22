using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace Kontur.GameStats.Server.Model
{
    public class PlayerStatistics
    {
        [BsonId]
        public string Name { get; set; }

        public int Matches { get; set; }

        public int Kills { get; set; }

        public int Deaths { get; set; }

        public double KillDeathRatio { get; set; }
    }
}
