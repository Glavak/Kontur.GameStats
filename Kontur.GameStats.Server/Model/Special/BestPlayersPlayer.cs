namespace Kontur.GameStats.Server.Model
{
    public class BestPlayersPlayer
    {
        public string Name { get; set; }

        public float KillToDeathRatio { get; set; }

        public BestPlayersPlayer(PlayerStatistics prototype)
        {
            Name = prototype.DisplayName;
            KillToDeathRatio = prototype.KillToDeathRatio;
        }
    }
}
