using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using Newtonsoft.Json;

namespace Kontur.GameStats.Server.Model
{
    public class Match
    {
        // Id is required for db to work properly
        [JsonIgnore]
        public int Id { get; set; }

        public string Server { get; set; }

        [BsonIndex(false)]
        public DateTime Timestamp { get; set; }

        public MatchResults Results { get; set; }
    }
}
