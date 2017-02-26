using System.Linq;

namespace Kontur.GameStats.Server.Model
{
    public class ServerStatsServer
    {
        public int TotalMatchesPlayed { get; set; }

        public int MaximumMathcesPerDay { get; set; }

        public float AverageMatchesPerDay { get; set; }

        public int MaximumPopulation { get; set; }

        public float AveragePopulation { get; set; }

        public string[] Top5GameModes { get; set; }

        public string[] Top5Maps { get; set; }

        public ServerStatsServer(Server prototype)
        {
            TotalMatchesPlayed = prototype.TotalMatchesPlayed;
            MaximumMathcesPerDay = prototype.MaximumMathcesPerDay;
            MaximumPopulation = prototype.MaximumPopulation;
            AveragePopulation = prototype.AveragePopulation;

            // Also count today's matches, that not counted in prototype.AverageMatchesPerDay
            AverageMatchesPerDay = MyMath.UpdateAverage(
                prototype.AverageMatchesPerDay, prototype.DaysActive, prototype.TodayMathcesPlayed);

            // Sort descending by values, and take first 5 for gameMode and map tops:
            var gameModesList = prototype.ServerGameModesCount.ToList();
            gameModesList.Sort((pair1, pair2) => -pair1.Value.CompareTo(pair2.Value));
            Top5GameModes = gameModesList.Select(x => x.Key).Take(5).ToArray();

            var mapsList = prototype.ServerMapsCount.ToList();
            mapsList.Sort((pair1, pair2) => -pair1.Value.CompareTo(pair2.Value));
            Top5Maps = mapsList.Select(x => x.Key).Take(5).ToArray();
        }
    }
}
