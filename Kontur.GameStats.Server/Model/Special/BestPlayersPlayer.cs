﻿namespace Kontur.GameStats.Server.Model
{
    public class BestPlayersPlayer
    {
        public string Name { get; set; }

        public float KillToDeathRatio { get; set; }

        public BestPlayersPlayer(PlayerStatistics prototype)
        {
            this.Name = prototype.Name;
            this.KillToDeathRatio = prototype.KillToDeathRatio;
        }
    }
}
