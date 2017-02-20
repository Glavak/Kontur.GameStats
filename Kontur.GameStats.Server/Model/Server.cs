using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server.Model
{
    public class servers
    {
        public string endpoint { get; set; }
        public string name { get; set; }
        public string game_modes { get; set; }
    }
}
