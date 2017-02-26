﻿namespace Kontur.GameStats.Server.Model
{
    public class PopularServersServer
    {
        public string Endpoint { get; set; }

        public string Name { get; set; }

        public float AverageMatchesPerDay { get; set; }

        public PopularServersServer(Server prototype)
        {
            Endpoint = prototype.Endpoint;
            Name = prototype.Info.Name;
            AverageMatchesPerDay = prototype.AverageMatchesPerDay;
        }
    }
}
