using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Model
{
    public class PopularServersServer
    {
        public string Endpoint { get; set; }

        public string Name { get; set; }

        public float AverageMatchesPerDay { get; set; }

        public PopularServersServer(Server prototype)
        {
            this.Endpoint = prototype.Endpoint;
            this.Name = prototype.Info.Name;
            this.AverageMatchesPerDay = prototype.AverageMatchesPerDay;
        }
    }
}
