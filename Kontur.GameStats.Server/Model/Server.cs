using LiteDB;
using System;
using System.Collections.Generic;

namespace Kontur.GameStats.Server.Model
{
    public class Server
    {
        // Id is required for db to work properly
        public int Id { get; set; }

        [BsonIndex]
        public string Endpoint { get; set; }

        public ServerInfo Info { get; set; }

        public DateTime AdvertisingTime { get; set; }

        public int TotalMatchesPlayed { get; set; }

        public int TodayMathcesPlayed { get; set; }

        public int MaximumMathcesPerDay { get; set; }

        [BsonIndex]
        public float AverageMatchesPerDay { get; set; }

        public int DaysActive { get; set; }

        public int MaximumPopulation { get; set; }

        public float AveragePopulation { get; set; }

        public DateTime LastMatchPlayed { get; set; }

        public Dictionary<string, int> ServerGameModesCount { get; set; }

        public Dictionary<string, int> ServerMapsCount { get; set; }

        public Server()
        {
            ServerGameModesCount = new Dictionary<string, int>();
            ServerMapsCount = new Dictionary<string, int>();
        }

        public void MatchPlayed(string gameMode, string map, int playersCount, DateTime time)
        {
            ServerGameModesCount.IncrementValue(gameMode);
            ServerMapsCount.IncrementValue(map);

            if (playersCount > MaximumPopulation)
            { 
                MaximumPopulation = playersCount;
            }
            AveragePopulation = MyMath.UpdateAverage(AveragePopulation, TotalMatchesPlayed, playersCount);

            TotalMatchesPlayed++;

            if (LastMatchPlayed.Day != time.Day)
            {
                // It's a new day, update average and reset today matches
                AverageMatchesPerDay = MyMath.UpdateAverage(AverageMatchesPerDay, DaysActive, TodayMathcesPlayed);
                DaysActive++;

                TodayMathcesPlayed = 1;
            }
            else
            {
                TodayMathcesPlayed++;
            }

            if (TodayMathcesPlayed > MaximumMathcesPerDay)
            {
                MaximumMathcesPerDay = TodayMathcesPlayed;
            }

            LastMatchPlayed = time;
        }
    }
}
