using LiteDB;
using Newtonsoft.Json;
using System;

namespace Kontur.GameStats.Server.Model
{
    public class Server
    {
        // Id is required for db to work properly
        [JsonIgnore]
        public int Id { get; set; }

        [BsonIndex]
        public string Endpoint { get; set; }

        public ServerInfo Info { get; set; }

        [JsonIgnore]
        public DateTime AdvertisingTime { get; set; }

        [JsonIgnore]
        public int MatchesCount { get; set; }

        [BsonIndex]
        [JsonIgnore]
        public float AverageMatchesPerDay { get; set; }
    }
}
