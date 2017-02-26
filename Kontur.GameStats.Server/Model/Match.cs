using System;
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
