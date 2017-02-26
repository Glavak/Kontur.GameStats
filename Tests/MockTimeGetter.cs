using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontur.GameStats.Server;

namespace Tests
{
    public class MockTimeGetter : ICurrentTimeGetter
    {
        public DateTime Time { get; set; }

        public MockTimeGetter(DateTime time)
        {
            Time = time;
        }

        public DateTime GetCurrentTime()
        {
            return Time;
        }
    }
}
